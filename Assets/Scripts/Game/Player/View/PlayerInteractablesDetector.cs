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
        private ContextContainer _contextContainer;
        
        public void Construct(Camera playerCamera, PlayerInput playerInput, ObjectHolder objectHolder)
        {
            _playerCamera = playerCamera;
            _playerInput = playerInput;
            _objectHolder = objectHolder;
            _contextContainer = new ContextContainer().
                AddContext(new ObjectHolderContext(_objectHolder));

            _playerInput.OnInteractButtonPressed.SubscribeWithSkip((() =>
            {
                if (_currentInteractable != null)
                    _currentInteractable.Interact(_contextContainer, Interaction.InteractButton);
            }));
            Observable.Interval(TimeSpan.FromSeconds(0.15f)).Subscribe((l =>
            {
                CheckForInteractables();
            }));
        }

        private void CheckForInteractables()
        {
            Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _interactDistance, _interactableLayer))
            {
                var newSelectedInteractable = hit.collider.GetComponent<InteractableView>();
                if(newSelectedInteractable == _currentInteractable) return;
                
                UnselectCurrentInteractable();
                _currentInteractable = newSelectedInteractable;
                _currentInteractable.Interact(_contextContainer, Interaction.OnLookStateChange);
                _currentInteractable.SetIsSelectedState(true);
            }
            else
            {
                UnselectCurrentInteractable();
            }
        }

        private void UnselectCurrentInteractable()
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.Interact(_contextContainer, Interaction.OnLookStateChange);
                _currentInteractable.SetIsSelectedState(false);
                _currentInteractable = null;
            }
        }
    }
}