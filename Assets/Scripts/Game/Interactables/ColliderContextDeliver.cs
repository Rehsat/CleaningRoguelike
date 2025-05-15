using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game.Interactables
{
    public class ColliderContextDeliver : MonoBehaviour
    {
        [SerializeField] private bool _constructOnStart;
        [SerializeField] private InteractableView _myInteractableView;
        [SerializeField] private Collider _collider;
        private ContextContainer _contextContainer;

        private void Start()
        {
            if(_constructOnStart)
                Construct();
        }

        public void Construct()
        {
            _collider.OnCollisionEnterAsObservable().Subscribe(collision =>
            {
                Debug.LogError(collision);
                if (collision.gameObject.TryGetComponent<InteractableView>(out var interactable))
                {
                    var collideContext = new CollidedInteractableContext(_myInteractableView);
                    _contextContainer = new ContextContainer()
                        .AddContext(collideContext);
                    
                    interactable.Interact(_contextContainer, Interaction.Collide);
                }
            });
        
        }

        public void AddContext(IContext context)
        {
            _contextContainer.AddContext(context);
        }
    }
}
