using Game.Interactables;
using UnityEngine;

namespace Game.Clothing
{
    public class ClothingSpawner : IAction
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
            var newClothing = _clothingFactory.Get();
            if(newClothing == null) return;
            
            newClothing.transform.position = _spawnPosition.position;
        }
    }
}