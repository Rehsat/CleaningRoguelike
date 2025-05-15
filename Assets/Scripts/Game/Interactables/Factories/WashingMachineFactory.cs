using EasyFramework.ReactiveEvents;
using Game.Clothing;
using Game.Interactables.Stackable;
using Gasme.Configs;
using UnityEngine;
using Zenject;

namespace Game.Interactables.Factories
{
    public class WashingMachineFactory : IFactory<ClothingChangerConfig, WashingMachine>, IContext
    {
        private readonly InteractablesMergeService _mergeService;
        private readonly WashingMachine _prefab;
        private ReactiveEvent<WashingMachine> _onWashingMachineCreated;
        
        public IReadOnlyReactiveEvent<WashingMachine> OnWashingMachineCreated => _onWashingMachineCreated;

        public WashingMachineFactory(PrefabsContainer prefabsContainer, InteractablesMergeService mergeService)
        {
            _mergeService = mergeService;
            _prefab = prefabsContainer.GetPrefabsComponent<WashingMachine>(Prefab.WashingMachine);
            _onWashingMachineCreated = new ReactiveEvent<WashingMachine>();
        }

        public WashingMachine Create(ClothingChangerConfig config)
        {
            var washingMachine = Object.Instantiate(_prefab);
            washingMachine.Construct();
            washingMachine
                .SetConfig(config);
            washingMachine
                .AddActionApplier(new TryMergeInteractablesAction(_mergeService), Interaction.Collide);
            //TODO добавить сюда инициализацию экшенов и контекстов
            
            _onWashingMachineCreated.Notify(washingMachine);
            return washingMachine;
        }
    }
}
