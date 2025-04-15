using EasyFramework.ReactiveEvents;
using Game.Clothing;
using UnityEngine;
using Zenject;

namespace Game.Interactables.Factories
{
    public class WashingMachineFactory : IFactory<ClothingChangerConfig, WashingMachine>
    {
        private readonly WashingMachine _prefab;
        private ReactiveEvent<WashingMachine> _onWashingMachineCreated;
        
        public IReadOnlyReactiveEvent<WashingMachine> OnWashingMachineCreated => _onWashingMachineCreated;

        public WashingMachineFactory(WashingMachine prefab)
        {
            _prefab = prefab;
            _onWashingMachineCreated = new ReactiveEvent<WashingMachine>();
        }

        public WashingMachine Create(ClothingChangerConfig config)
        {
            var washingMachine = Object.Instantiate(_prefab);
            //washingMachine.SetConfig(config); //TODO добавить сюда инициализацию экшенов и контекстов
            
            _onWashingMachineCreated.Notify(washingMachine);
            return washingMachine;
        }
    }
}
