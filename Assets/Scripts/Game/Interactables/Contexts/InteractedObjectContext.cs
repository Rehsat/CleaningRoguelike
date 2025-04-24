namespace Game.Interactables
{
    public class InteractedObjectContext : IContext
    {
        public InteractableView InteractableView { get; }

        public InteractedObjectContext(InteractableView interactableView)
        {
            InteractableView = interactableView;
        }
    }
}