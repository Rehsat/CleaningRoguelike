using System;
using Game.Interactables.Stackable;
using UnityEngine;

namespace Game.Interactables
{
    public class DoSimpleAction : IAction
    {
        private readonly Action _action;

        public DoSimpleAction(Action action)
        {
            _action = action;
        }
        public void ApplyAction(ContextContainer context)
        {
            _action.Invoke();
        }
    }

    public class TryMergeInteractablesAction : IAction
    {
        private readonly InteractablesMergeService _mergeService;

        public TryMergeInteractablesAction(InteractablesMergeService mergeService)
        {
            _mergeService = mergeService;
        }

        public void ApplyAction(ContextContainer context)
        {
            Debug.LogError(123);
            InteractedObjectContext firstInteractedObjectContext;
            CollidedInteractableContext collidedInteractableContext;
            if (context.TryGetContext(out firstInteractedObjectContext) 
                && context.TryGetContext(out collidedInteractableContext))
            {
                _mergeService.Merge(firstInteractedObjectContext.InteractableView,
                    collidedInteractableContext.CollidedInteractableView);
            }
        }
    }
}