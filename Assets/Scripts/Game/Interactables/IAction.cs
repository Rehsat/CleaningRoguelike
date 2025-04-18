using EasyFramework.ReactiveEvents;

namespace Game.Interactables
{
    public interface IAction
    {
        public void ApplyAction(ContextContainer context);
    }

    public interface IWorkAction : IAction, IProgressModelContainer
    {
        public IReadOnlyReactiveEvent<WorkState> OnWorkStateChanged { get; }
    }
}