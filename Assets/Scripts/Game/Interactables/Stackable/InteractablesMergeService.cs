using UnityEngine;

namespace Game.Interactables.Stackable
{
    public class InteractablesMergeService
    {
        public void Merge(InteractableView firstInteractable, InteractableView secondInteractable)
        {
            firstInteractable.Stack(secondInteractable);
            firstInteractable.transform.DoShowAnimation(secondsDuration:0.6f, withOriginalScale: true);
            
            Object.Destroy(secondInteractable.gameObject);
        }
    }
}