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
        [Header("Raycast Settings")]
        [SerializeField] private float _interactDistance = 3f;
        [SerializeField] private float _coneAngle = 45;

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
            _contextContainer = new ContextContainer(). //TODO переписать чтоб логика была на более высоком слое
                AddContext(new ObjectHolderContext(_objectHolder));

            _playerInput.OnInteractButtonPressed.SubscribeWithSkip((value =>
            {
                if(value == false) return;
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
                
                SelectInteractable(newSelectedInteractable);
            }
            else
            {
                UnselectCurrentInteractable();
                var interactableInCone = GetInteractablesWithCone();
                if(interactableInCone != null)
                    SelectInteractable(interactableInCone);
            }
        }

        private void SelectInteractable(InteractableView newSelectedInteractable)
        {
            if(newSelectedInteractable == _currentInteractable) return;
            
            UnselectCurrentInteractable();
            
            _currentInteractable = newSelectedInteractable;
            _currentInteractable.Interact(_contextContainer, Interaction.OnLookStateChange);
            _currentInteractable.SetIsSelectedState(true);
        }

        private InteractableView GetInteractablesWithCone()
        {
            Vector3 cameraPos = _playerCamera.transform.position;
            Vector3 cameraForward = _playerCamera.transform.forward;
            Collider[] allHits = Physics.OverlapSphere(cameraPos, _interactDistance, _interactableLayer);

            InteractableView closestInteractable = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider col in allHits)
            {
                Vector3 dirToObj = (col.transform.position - cameraPos).normalized;
                float angle = Vector3.Angle(cameraForward, dirToObj);

                if (angle <= _coneAngle / 2f)
                {
                    float distance = Vector3.Distance(cameraPos, col.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestInteractable = col.GetComponent<InteractableView>();
                        if (closestInteractable.AimHelpEnabled)
                            return closestInteractable;
                    }
                }
            }

            return null;
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