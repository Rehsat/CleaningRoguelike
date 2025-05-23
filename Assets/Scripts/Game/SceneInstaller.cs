using Game.Clothing;
using Game.Configs;
using Game.GameStateMachine;
using Game.Interactables;
using Game.Interactables.Contexts;
using Game.Interactables.Factories;
using Game.Interactables.Stackable;
using Game.Player.Data;
using Game.Player.PayerInput;
using Game.Player.View;
using Game.Quota;
using Game.Services;
using Game.UI.Resources;
using Game.Upgrades;
using Gasme.Configs;
using UnityEngine;
using Zenject;

namespace Game
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private GameGlobalConfig _globalConfig;
        [SerializeField] private SceneObjectsContainer _sceneObjects;
        [SerializeField] private BounceAnimator _bounceAnimator;
        public override void InstallBindings()
        {
            InstallConfigs(_globalConfig);
            
            Container.BindInstance(_sceneObjects).AsSingle();
            Container.BindInstance(_bounceAnimator).AsSingle();

            Container.Bind<InteractablesMergeService>().FromNew().AsSingle();
            InstallFactories();
            
            Container.Bind<PlayerInput>().FromNew().AsSingle();
            Container.Bind<ObjectHolder>().FromNew().AsSingle();
            Container.Bind<GameValuesContainer>().FromNew().AsSingle();
            Container.Bind<GameValueChangeObserver>().FromNew().AsSingle().NonLazy();

            InstallUpgrades();
            InstallServices();
            
            Container.Bind<ObjectSeller>().FromNew().AsSingle();
            Container.Bind<QuotaCostManager>().FromNew().AsSingle();
            
            InstallGlobalContext();
            
            InstallStateMachine();
        }

        private void InstallConfigs(GameGlobalConfig globalConfig)
        {
            Container.BindInterfacesAndSelfTo<PrefabsContainer>().FromInstance(globalConfig.PrefabsContainer).AsSingle();
            Container.BindInstance(globalConfig.ResourceConfigsList).AsSingle();
            Container.BindInstance(globalConfig).AsSingle();
        }
        private void InstallFactories()
        {
            Container.BindInterfacesAndSelfTo<WashingMachineFactory>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<BuildableBoxesFactory>().FromNew().AsSingle();
        }

        private void InstallUpgrades()
        {
            var upgradeView =_sceneObjects.GetObjectsComponent<IUpgradeView>(SceneObject.UpgradeView);
            
            Container.Bind<UpgradesSelector>().FromNew().AsSingle();
            Container.BindInstance(upgradeView).AsSingle();
            Container.BindInterfacesAndSelfTo<UpgradesController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UpgradesShowStateController>().AsSingle().NonLazy();
        }

        private void InstallServices()
        {
            Container.Bind<CursorEnableService>().FromNew().AsSingle().NonLazy();
        }
        private void InstallStateMachine()
        {
            Container.Bind<ILevelState>().To<BootstrapState>().FromNew().AsSingle().NonLazy();
            Container.Bind<ILevelState>().To<UpgradeGameState>().FromNew().AsSingle();
            Container.Bind<ILevelState>().To<DoWorkState>().FromNew().AsSingle();

            Container.Bind<GameStateMachine.GameStateMachine>().FromNew().AsSingle().NonLazy();
            Container.Bind<CurrentGameStateObserver>().FromNew().AsSingle();
        }

        private void InstallGlobalContext()
        {
            Container.Bind<IContext>().To<GameValuesContext>().AsSingle();
            Container.Bind<IContext>().To<SpawnTransformContext>()
                .FromInstance(new SpawnTransformContext(_spawnPosition)).AsSingle();
            
            Container.Bind<GlobalContextContainer>().FromNew().AsSingle().NonLazy();
        }

    }
}