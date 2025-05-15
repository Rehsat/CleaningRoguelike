
using System;

namespace Game.Interactables
{
    public interface IActionsContainer
    {
        bool TryGetAction(Type actionType, out IAction action);
    }
}