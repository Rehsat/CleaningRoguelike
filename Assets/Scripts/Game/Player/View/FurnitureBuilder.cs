using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Player.Data;
using Game.Player.PayerInput;
using Game.Player.View;
using UniRx;
using UnityEditor;
using UnityEngine;

public class FurnitureBuilder : MonoBehaviour
{
    //TODO: сделать настройки в конструкторе
    [Header("Settings")]
    [SerializeField] private float _maxPlacementDistance = 5f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _obstructionMask;
    [SerializeField] private Material _validMaterial;
    [SerializeField] private Material _invalidMaterial;
    [SerializeField] private ParticleSystem _buildParticle;

    private ObjectHolder _objectHolder;
    private PlayerInput _playerInput;
    private GameObject _currentPreview;
    private Coroutine _placementCoroutine;
    private FurnitureContainerBox _currentBox;
    private CompositeDisposable _holdableItemDisposable;
    private Sequence _rotateSequence;
    private bool IsBuildingAvailable => _currentPreview != null && CheckCollisions();

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
        _playerInput.OnBuildingRotatePerformed.SubscribeWithSkip(RotateConstructionPreview);
        
        _objectHolder.CurrentPickedUpObject
            .Subscribe(HandleNewHoldableObject)
            .AddTo(this);
    }

    private void HandleNewHoldableObject(Rigidbody newObject)
    {
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
    private void RotateConstructionPreview(Vector2 rotationDirection)
    {
        if(_currentPreview == null) return;
        _rotateSequence?.Kill();
        _rotateSequence = DOTween.Sequence();
        
        var rotationStep = 90;
        var animationSpeed = 0.3f;
        var newRotation = _currentPreview.transform.eulerAngles + (Vector3)rotationDirection * rotationStep;
        var tween = _currentPreview.transform
            .DORotate(newRotation, animationSpeed)
            .SetEase(Ease.OutBack);

        _rotateSequence.Append(tween)
            .OnKill((() => _currentPreview.transform.eulerAngles = newRotation));
        _rotateSequence.Play();
    }

    private void CreatePreviewObject()
    {
        _currentPreview = Instantiate(_currentBox.BuildableObjectPrefab);
        ToggleColliders(_currentPreview, false);
    }

    private void SetPreviewMaterial(GameObject obj, Material material)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            var materials = r.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] =  material;
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
        while (_currentPreview != null)
        {
            UpdatePreviewPosition();
            var isBuildAvailable = IsBuildingAvailable;
            var material = isBuildAvailable ? _validMaterial : _invalidMaterial;
            SetPreviewMaterial(_currentPreview, material);
            yield return null;
            yield return null;
            yield return null;
            yield return null;
        }
    }

    private void UpdatePreviewPosition()
    {
        Ray ray = new Ray(transform.position + transform.forward * 0.5f, transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, _maxPlacementDistance, _groundMask))
        {
            _currentPreview.transform.position = (hit.point + hit.normal * 0.05f); 
        }
        else
        {
            _currentPreview.transform.position = ray.GetPoint(_maxPlacementDistance);
        }

        _currentPreview.transform.position = _currentPreview.transform.position.SnapToGrid();
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
        _rotateSequence?.Kill();
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
        
        finalObject.transform.DoShowAnimation(0.5f,true);
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
    private bool CheckCollisions()
    {
        if (_currentPreview == null) return false;

        var collisions = Physics.OverlapBox(
            _currentPreview.transform.position,
            _currentPreview.transform.localScale/2, 
            _currentPreview.transform.rotation,
            _obstructionMask
        ).Where(c => !c.transform.IsChildOf(_currentPreview.transform)).ToArray();
        
        return collisions.Length == 0;
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
