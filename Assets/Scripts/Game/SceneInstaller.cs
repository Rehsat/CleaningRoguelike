using Game.Player.Data;
using Game.Player.PayerInput;
using Zenject;

namespace Game
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerInput>().FromNew().AsSingle();
            Container.Bind<ObjectHolder>().FromNew().AsSingle();
        }
    }
}