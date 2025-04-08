using Game.Clothing;
using Game.Player.Data;
using Game.Player.PayerInput;
using UnityEngine;
using Zenject;

namespace Game
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private PrefabsContainer _prefabsContainer;
        public override void InstallBindings()
        {
            Container.BindInstance(_prefabsContainer).AsSingle();
            
            Container.Bind<PlayerInput>().FromNew().AsSingle();
            Container.Bind<ObjectHolder>().FromNew().AsSingle();
            Container.Bind<PlayerResources>().FromNew().AsSingle();
            
            Container.Bind<ObjectSeller>().FromNew().AsSingle();
        }
    }
}