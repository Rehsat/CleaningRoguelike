using Game.Interactables;
using UnityEngine;

namespace Game.Clothing
{
    public class ClothingSpawner : IActionContainer
    {
        private readonly ClothingFactory _clothingFactory;
        private readonly Transform _spawnPosition;

        public ClothingSpawner(ClothingFactory clothingFactory, Transform spawnPosition)
        {
            _clothingFactory = clothingFactory;
            _spawnPosition = spawnPosition;
        }
        public void ApplyAction(ContextContainer context)
        {
            _clothingFactory.CreateClothing(_spawnPosition.position);
        }
    }
}