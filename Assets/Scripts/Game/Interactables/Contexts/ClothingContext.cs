using Game.Clothing;

namespace Game.Interactables
{
    public class ClothingContext : IInteractableContext
    {
        public ClothingView Clothing { get; }

        public ClothingContext(ClothingView clothing)
        {
            Clothing = clothing;
        }
    }
}