using EasyFramework.ReactiveTriggers;
using Game.Interactables;
using Game.Interactables.Contexts;
using UnityEngine;

namespace Game.Clothing
{
    public class ClothingFactory : IFactory<ClothingView>
    {
        private readonly ClothingView _clothingPrefab;
        private readonly ResourceData _activeClothingResource;

        public ClothingFactory(ClothingView clothingPrefab, ResourceData activeClothingResource)
        {
            _clothingPrefab = clothingPrefab;
            _activeClothingResource = activeClothingResource;
        }

        public ClothingView Get()
        {
            if (_activeClothingResource.IsCurrentValueMaximum)
                return null;

            var onReturnTrigger = new ReactiveTrigger();
            var newClothing = Object.Instantiate(_clothingPrefab);
            
            newClothing.Construct();
            newClothing
                .AddActionApplier(new PickUpAction())
                .AddContext(new SellableContext(1))
                .AddContext(new ReturnableContext(onReturnTrigger));
            
            _activeClothingResource.ChangeValueBy(1);
            onReturnTrigger.SubscribeWithSkip(() => _activeClothingResource.ChangeValueBy(-1));
            return newClothing;
        }
    }
}
