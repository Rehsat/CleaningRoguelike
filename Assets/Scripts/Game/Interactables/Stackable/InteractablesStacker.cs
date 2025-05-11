namespace Game.Interactables.Stackable
{
    public class InteractablesStacker
    {
        public void Stack(InteractableView firstInteractable, InteractableView secondInteractable)
        {
            firstInteractable.Stack(secondInteractable);
        }
    }
}