using Game.Clothing;
using Game.GameStateMachine;
using Game.Player.Data;
using Game.Player.PayerInput;
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
            
            Container.Bind<PlayerInput>().FromNew().AsSingle();
            Container.Bind<ObjectHolder>().FromNew().AsSingle();
            Container.Bind<PlayerResources>().FromNew().AsSingle();
            
            Container.Bind<ObjectSeller>().FromNew().AsSingle();
            
            InstallStateMachine();
        }

        private void InstallStateMachine()
        {
            Container.Bind<ILevelState>().To<UpgradeState>().AsSingle();
            Container.Bind<ILevelState>().To<WorkState>().AsSingle();

            Container.Bind<GameStateMachine.GameStateMachine>().FromNew().AsSingle();
        }
    }
}