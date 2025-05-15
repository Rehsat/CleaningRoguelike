using UnityEngine;

namespace Game.Interactables
{
    public class CollidedInteractableContext : IContext
    {
        public InteractableView CollidedInteractableView { get; }

        public CollidedInteractableContext(InteractableView collidedInteractableView)
        {
            CollidedInteractableView = collidedInteractableView;
        }
    }
}