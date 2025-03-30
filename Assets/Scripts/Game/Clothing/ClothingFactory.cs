using Game.Interactables;
using Game.Interactables.Contexts;
using UnityEngine;

namespace Game.Clothing
{
    public class ClothingFactory : IFactory<ClothingView>
    {
        private readonly ClothingView _clothingPrefab;

        public ClothingFactory(ClothingView clothingPrefab)
        {
            _clothingPrefab = clothingPrefab;
        }

        public ClothingView Get()
        {
            var newClothing = Object.Instantiate(_clothingPrefab);
            newClothing.Construct();
            newClothing
                .AddActionApplier(new PickUpAction())
                .AddContext(new SellableContext(1));
            return newClothing;
        }
    }
}
