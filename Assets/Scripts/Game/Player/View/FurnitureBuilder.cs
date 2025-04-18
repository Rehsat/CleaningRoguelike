using System.Collections;
using System.Collections.Generic;
using Game.Player.Data;
using Game.Player.PayerInput;
using UniRx;
using UnityEngine;

public class FurnitureBuilder : MonoBehaviour
{
    //TODO: сделать настройки в конструкторе
    [Header("Settings")]
    [SerializeField] private float _placementOffset = 1.5f;
    [SerializeField] private float _maxPlacementDistance = 5f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Material _previewMaterial;
    [SerializeField] private ParticleSystem _buildParticle;

    private ObjectHolder _objectHolder;
    private PlayerInput _playerInput;
    private GameObject _currentPreview;
    private Coroutine _placementCoroutine;
    private FurnitureContainerBox _currentBox;
    private CompositeDisposable _holdableItemDisposable;
    
    private bool IsBuildingAvailable => _currentPreview != null;

    public void Construct(ObjectHolder holder, PlayerInput playerInput)
    {
        _objectHolder = holder;
        _playerInput = playerInput;
        _holdableItemDisposable = new CompositeDisposable();
        _buildParticle = Instantiate(_buildParticle);

        _playerInput.OnInteractButtonPressed
            .SubscribeWithSkip((isPressed =>
            {
                if (isPressed) HandlePlacementInput();
            }));
        
        _objectHolder.CurrentPickedUpObject
            .Subscribe(HandleNewHoldableObject)
            .AddTo(this);
    }

    private void HandleNewHoldableObject(Rigidbody newObject)
    {
        Debug.LogError(321);
        Observable.TimerFrame(1).Subscribe((l =>
        {
            ClearPreview();
            if (newObject == null) return;
        
            UpdateConstructionPreview(newObject);
        }));
    }
    private void UpdateConstructionPreview(Rigidbody newObject)
    {

        _currentBox = newObject.GetComponent<FurnitureContainerBox>();
        if (_currentBox?.BuildableObjectPrefab == null) return;

        CreatePreviewObject();
        StartPlacementTracking();
    }

    private void CreatePreviewObject()
    {
        _currentPreview = Instantiate(_currentBox.BuildableObjectPrefab);
        SetPreviewMaterial(_currentPreview);
        ToggleColliders(_currentPreview, false);
    }

    private void SetPreviewMaterial(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            var materials = r.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = _previewMaterial;
            }
            r.materials = materials;
        }
    }

    private void StartPlacementTracking()
    {
        if (_placementCoroutine != null) StopCoroutine(_placementCoroutine);
        _placementCoroutine = StartCoroutine(TrackPlacementPosition());
    }

    private IEnumerator TrackPlacementPosition()
    {
        while (IsBuildingAvailable)
        {
            UpdatePreviewPosition();
            yield return null;
        }
    }

    private void UpdatePreviewPosition()
    {
        Ray ray = new Ray(transform.position + transform.forward * 0.5f, transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, _maxPlacementDistance, _groundMask))
        {
            _currentPreview.transform.position = (hit.point + hit.normal * 0.05f); 
            _currentPreview.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
        else
        {
            _currentPreview.transform.position = ray.GetPoint(_maxPlacementDistance);
            _currentPreview.transform.rotation = Quaternion.identity;
        }

        _currentPreview.transform.position = _currentPreview.transform.position.SnapToGrid();
    }

    private void HandlePlacementInput()
    {
        Debug.LogError(123);
        if (IsBuildingAvailable)
        {
            PlaceObject();
            _objectHolder.ForceRemoveCurrentObject();
        }
    }

    private void PlaceObject()
    {
        var buildPosition = _currentPreview.transform.position;
        var finalObject = Instantiate(
            _currentBox.BuildableObjectPrefab,
            buildPosition,
            _currentPreview.transform.rotation
        );
        _buildParticle.transform.position = new Vector3(
            buildPosition.x,
            buildPosition.y ,
            buildPosition.z);
        _buildParticle.Play();
        
        finalObject.transform.DoShowAnimation(withOriginalScale: true);
        ToggleColliders(finalObject, true);
        ClearPreview();
    }

    private void ToggleColliders(GameObject obj, bool state)
    {
        foreach (var collider in obj.GetComponentsInChildren<Collider>())
        {
            collider.enabled = state;
        }
    }

    private void ClearPreview()
    {
        _holdableItemDisposable?.Dispose();
        _holdableItemDisposable = new CompositeDisposable();
        
        if (_currentPreview != null)
        {
            Destroy(_currentPreview);
            _currentPreview = null;
        }
        
        if (_placementCoroutine != null)
        {
            StopCoroutine(_placementCoroutine);
            _placementCoroutine = null;
        }
    }
}
