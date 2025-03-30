namespace Game.Interactables
{
    public interface IContextContainer
    {
        public ContextContainer AddContext<TContext>(TContext context) where TContext : IInteractableContext;
        public bool TryGetContext<TContext>(out TContext context) where TContext : IInteractableContext;
    }
    public interface IInteractableContext{}
}