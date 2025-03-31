using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game.Interactables
{
    public class ColliderContextDeliver : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        private ContextContainer _contextContainer;

        public void Construct()
        {
            _contextContainer = new ContextContainer();
            _collider.OnCollisionEnterAsObservable().Subscribe(collision =>
            {
                if (collision.gameObject.TryGetComponent<InteractableView>(out var interactable))
                {
                    interactable.Interact(_contextContainer, Interaction.Collide);
                }
            });
        
        }

        public void AddContext(IInteractableContext context)
        {
            _contextContainer.AddContext(context);
        }
    }
}
