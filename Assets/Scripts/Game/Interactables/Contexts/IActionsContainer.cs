
namespace Game.Interactables
{
    public interface IActionsContainer
    {
        public bool TryGetAction<TActionType>(out TActionType action) where TActionType : IAction;
    }
}