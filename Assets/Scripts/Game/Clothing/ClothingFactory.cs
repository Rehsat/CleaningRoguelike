using EasyFramework.ReactiveTriggers;
using Game.Interactables;
using Game.Interactables.Contexts;
using UnityEngine;
using Zenject;

namespace Game.Clothing
{
    public class ClothingFactory : IFactory<ClothingView>
    {
        private readonly ClothingView _clothingPrefab;
        private readonly PlayerGameValueData _activeClothingPlayerGameValue;

        public ClothingFactory(ClothingView clothingPrefab, PlayerGameValueData activeClothingPlayerGameValue)
        {
            _clothingPrefab = clothingPrefab;
            _activeClothingPlayerGameValue = activeClothingPlayerGameValue;
        }

        public ClothingView Create()
        {
            if (_activeClothingPlayerGameValue.IsCurrentValueMaximum)
                return null;

            var onReturnTrigger = new ReactiveTrigger();
            var newClothing = Object.Instantiate(_clothingPrefab);
            
            newClothing.Construct();
            newClothing
                .AddActionApplier(new PickUpAction())
                .AddContext(new SellableContext(1))
                .AddContext(new ReturnableContext(onReturnTrigger));
            
            _activeClothingPlayerGameValue.ChangeValueBy(1);
            onReturnTrigger.SubscribeWithSkip(() => _activeClothingPlayerGameValue.ChangeValueBy(-1));
            return newClothing;
        }
    }
}
