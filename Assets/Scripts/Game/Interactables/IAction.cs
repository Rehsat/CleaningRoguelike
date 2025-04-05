using EasyFramework.ReactiveEvents;

namespace Game.Interactables
{
    public interface IAction
    {
        public void ApplyAction(ContextContainer context);
    }

    public interface IWorkAction : IAction
    {
        public IReadOnlyReactiveEvent<bool> OnWorkStateChanged { get; }
    }
}