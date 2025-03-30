using System;
using Game.Interactables;
using Game.Interactables.Contexts;
using Game.Player.Data;
using Game.Player.PayerInput;
using UniRx;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInteractablesDetector: MonoBehaviour
    {
        [Header("Raycast Settings")] public float _interactDistance = 3f;

        public LayerMask _interactableLayer;
        private InteractableView _currentInteractable;
        private Camera _playerCamera;
        private PlayerInput _playerInput;
        private ObjectHolder _objectHolder;
        
        public void Construct(Camera playerCamera, PlayerInput playerInput, ObjectHolder objectHolder)
        {
            _playerCamera = playerCamera;
            _playerInput = playerInput;
            _objectHolder = objectHolder;

            _playerInput.OnInteractButtonPressed.SubscribeWithSkip((() =>
            {
                var contextContainer = new ContextContainer().
                    AddContext(new ObjectHolderContext(_objectHolder));
                
                if (_currentInteractable != null)
                    _currentInteractable.Interact(contextContainer);
            }));
            Observable.Interval(TimeSpan.FromSeconds(0.15f)).Subscribe((l =>
            {
                CheckForInteractables();
            }));
        }

        void CheckForInteractables()
        {
            Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _interactDistance, _interactableLayer))
            {
                ResetCurrentInteractable();
                _currentInteractable = hit.collider.GetComponent<InteractableView>();
                _currentInteractable.SetIsSelectedState(true);
            }
            else
            {
                ResetCurrentInteractable();
                _currentInteractable = null;
            }
        }

        private void ResetCurrentInteractable()
        {
            if(_currentInteractable != null)
                _currentInteractable.SetIsSelectedState(false);
        }
    }
}