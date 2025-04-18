using System.Collections;
using System.Collections.Generic;
using Game.Player.Data;
using Game.Player.PayerInput;
using UniRx;
using UnityEngine;

public class FurnitureBuilder : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _placementOffset = 1.5f;
    [SerializeField] private float _maxPlacementDistance = 5f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Material _previewMaterial;

    private ObjectHolder _objectHolder;
    private PlayerInput _playerInput;
    private GameObject _currentPreview;
    private Coroutine _placementCoroutine;
    private FurnitureContainerBox _currentBox;
    
    private bool IsBuildingAvailable => _currentPreview != null;

    public void Construct(ObjectHolder holder, PlayerInput playerInput)
    {
        _objectHolder = holder;
        _playerInput = playerInput;
        
        _objectHolder.CurrentPickedUpObject
            .Subscribe(UpdateConstructionPreview)
            .AddTo(this);

        playerInput.OnInteractButtonPressed.SubscribeWithSkip((b =>
            HandlePlacementInput()));
    }

    private void UpdateConstructionPreview(Rigidbody newObject)
    {
        ClearPreview();

        if (newObject == null) return;

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
            _currentPreview.transform.position = hit.point + hit.normal * 0.05f;
            _currentPreview.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
        else
        {
            _currentPreview.transform.position = ray.GetPoint(_maxPlacementDistance);
            _currentPreview.transform.rotation = Quaternion.identity;
        }
    }

    private void HandlePlacementInput()
    {
        if (IsBuildingAvailable)
        {
            PlaceObject();
            _objectHolder.ForceRemoveCurrentObject();
        }
    }

    private void PlaceObject()
    {
        var finalObject = Instantiate(
            _currentBox.BuildableObjectPrefab,
            _currentPreview.transform.position,
            _currentPreview.transform.rotation
        );

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
