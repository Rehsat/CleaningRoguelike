using Zenject;

namespace Game.Interactables
{
    public interface IContextContainer
    {
        public ContextContainer AddContext<TContext>(TContext context) where TContext : IContext;
        public bool TryGetContext<TContext>(out TContext context) where TContext : IContext;
    }
    public interface IContext{}
}