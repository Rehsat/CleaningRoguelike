using Game.Interactables;
using UnityEngine;

namespace Game.Clothing
{
    public class ClothingFactory 
    {
        private readonly ClothingView _clothingPrefab;

        public ClothingFactory(ClothingView clothingPrefab)
        {
            _clothingPrefab = clothingPrefab;
        }

        public ClothingView CreateClothing(Vector3 position)
        {
            var newClothing = Object.Instantiate(_clothingPrefab, position, Quaternion.identity);
            newClothing.AddActionApplier(new PickUpActionContainer());
            return newClothing;
        }
    }
}
