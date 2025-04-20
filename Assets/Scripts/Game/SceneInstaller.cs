using Game.Clothing;
using Game.GameStateMachine;
using Game.Interactables.Factories;
using Game.Player.Data;
using Game.Player.PayerInput;
using Game.Quota;
using UnityEngine;
using Zenject;

namespace Game
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private PrefabsContainer _prefabsContainer;
        [SerializeField] private SceneObjectsContainer _sceneObjects;
        public override void InstallBindings()
        {
            Container.BindInstance(_prefabsContainer).AsSingle();
            Container.BindInstance(_sceneObjects).AsSingle();
            
            InstallFactories();
            
            Container.Bind<PlayerInput>().FromNew().AsSingle();
            Container.Bind<ObjectHolder>().FromNew().AsSingle();
            Container.Bind<PlayerResources>().FromNew().AsSingle();
            
            Container.Bind<ObjectSeller>().FromNew().AsSingle();
            Container.Bind<QuotaCostManager>().FromNew().AsSingle();
            InstallStateMachine();
        }

        private void InstallFactories()
        {
            var washingMachinePrefab = _prefabsContainer.GetPrefabsComponent<WashingMachine>(Prefab.WashingMachine);
            Container.BindInstance(new WashingMachineFactory(washingMachinePrefab)).AsSingle();
        }
        private void InstallStateMachine()
        {
            Container.Bind<ILevelState>().To<UpgradeState>().AsSingle();
            Container.Bind<ILevelState>().To<DoWorkState>().AsSingle();

            Container.Bind<GameStateMachine.GameStateMachine>().FromNew().AsSingle();
            Container.Bind<CurrentGameStateObserver>().FromNew().AsSingle();
        }
    }
}