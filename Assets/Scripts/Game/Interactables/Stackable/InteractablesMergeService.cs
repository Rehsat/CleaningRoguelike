using Game.Player.View;
using UnityEngine;

namespace Game.Interactables.Stackable
{
    public class InteractablesMergeService
    {
        public void Merge(InteractableView firstInteractable, InteractableView secondInteractable)
        {
            InteractableView interactableToStack = secondInteractable;
            if (secondInteractable is FurnitureContainerBox furnitureContainerBox)
                if (furnitureContainerBox.BuildableObjectPrefab.TryGetComponent<InteractableView>(
                    out var buildInteractable))
                    interactableToStack = buildInteractable;
            
            firstInteractable.Stack(interactableToStack);
            firstInteractable.transform.DoShowAnimation(secondsDuration:0.6f, withOriginalScale: true);
            
            Object.Destroy(secondInteractable.gameObject);
        }
    }
}