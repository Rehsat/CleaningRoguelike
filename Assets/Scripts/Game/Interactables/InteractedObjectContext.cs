using Game.Player.Data;

namespace Game.Interactables
{
    public class InteractedObjectContext : IInteractableContext
    {
        public InteractableView InteractableView { get; }

        public InteractedObjectContext(InteractableView interactableView)
        {
            InteractableView = interactableView;
        }
    }

    public class PlayerObjectHolderContext : IInteractableContext
    {
        public ObjectHolder ObjectHolder { get; }

        public PlayerObjectHolderContext(ObjectHolder objectHolder)
        {
            ObjectHolder = objectHolder;
        }
    }
}