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
}