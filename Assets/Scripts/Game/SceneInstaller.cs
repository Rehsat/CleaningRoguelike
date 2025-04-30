using Game.Clothing;
using Game.Configs;
using Game.GameStateMachine;
using Game.Interactables;
using Game.Interactables.Contexts;
using Game.Interactables.Factories;
using Game.Player.Data;
using Game.Player.PayerInput;
using Game.Quota;
using Game.UI.Resources;
using Game.Upgrades;
using Gasme.Configs;
using UnityEngine;
using Zenject;

namespace Game
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private GameGlobalConfig _globalConfig;
        [SerializeField] private SceneObjectsContainer _sceneObjects;
        [SerializeField] private BounceAnimator _bounceAnimator;
        private GlobalContextContainer _globalContextContainer;
        public override void InstallBindings()
        {
            InstallConfigs(_globalConfig);
            
            Container.BindInstance(_sceneObjects).AsSingle();
            Container.BindInstance(_bounceAnimator).AsSingle();
            
            InstallFactories();
            
            Container.Bind<PlayerInput>().FromNew().AsSingle();
            Container.Bind<ObjectHolder>().FromNew().AsSingle();
            Container.Bind<GameValuesContainer>().FromNew().AsSingle();
            Container.Bind<GameValueChangeObserver>().FromNew().AsSingle().NonLazy();

            Container.Bind<UpgradesSelector>().FromNew().AsSingle();
            Container.Bind<UpgradeController>().FromNew().AsSingle();

            Container.Bind<ObjectSeller>().FromNew().AsSingle();
            Container.Bind<QuotaCostManager>().FromNew().AsSingle();
            
            InstallGlobalContext();
            
            InstallStateMachine();
        }

        private void InstallConfigs(GameGlobalConfig globalConfig)
        {
            Container.BindInstance(globalConfig.PrefabsContainer).AsSingle();
            Container.BindInstance(globalConfig.ResourceConfigsList).AsSingle();
        }
        private void InstallFactories()
        {
            var washingMachinePrefab = _globalConfig.PrefabsContainer.GetPrefabsComponent<WashingMachine>(Prefab.WashingMachine);
            Container.BindInstance(new WashingMachineFactory(washingMachinePrefab)).AsSingle();
        }
        private void InstallStateMachine()
        {
            Container.Bind<ILevelState>().To<UpgradeGameState>().AsSingle();
            Container.Bind<ILevelState>().To<DoWorkState>().AsSingle();

            Container.Bind<GameStateMachine.GameStateMachine>().FromNew().AsSingle();
            Container.Bind<CurrentGameStateObserver>().FromNew().AsSingle();
        }

        private void InstallGlobalContext()
        {
            Container.Bind<IContext>().To<GameValuesContext>().AsSingle();
            
            Container.BindInstance(_globalContextContainer).AsSingle().NonLazy();
        }

    }
}