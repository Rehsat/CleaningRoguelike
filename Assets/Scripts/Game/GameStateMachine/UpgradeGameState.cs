using Game.Interactables;
using Game.Interactables.Contexts;
using Game.Upgrades;
using UniRx;
using Zenject;

namespace Game.GameStateMachine
{
    public class UpgradeGameState : ILevelState
    {
        private readonly InteractableButton _startQuotaButton;

        private CompositeDisposable _compositeDisposable;
        private GameStateMachine _stateMachine;

        [Inject]
        public UpgradeGameState(SceneObjectsContainer sceneObjects)
        {
            _startQuotaButton = sceneObjects.GetObjectsComponent<InteractableButton>(SceneObject.QuotaStartButton);
            _startQuotaButton.Construct();
        }
        public void SetStateMachine(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public void Enter()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = new CompositeDisposable();
            
            _startQuotaButton.SetEnabled(true);
            _startQuotaButton.OnInteracted.SubscribeWithSkip((() =>
            {
                _stateMachine.EnterState<DoWorkState>();
            })).AddTo(_compositeDisposable);
        }

        public void Exit()
        {
            _startQuotaButton.SetEnabled(false);
            _compositeDisposable?.Dispose();
        }

        public void OnUpdate()
        {
        }
    }

    public class BootstrapState : ILevelState
    {
        public BootstrapState(UpgradeController upgradeController, SceneObjectsContainer sceneObjectsContainer, GlobalContextContainer globalContextContainer)
        {
            
        }
        public void Enter()
        {
            
        }

        public void Exit()
        {
        }


        public void SetStateMachine(GameStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }
        public void OnUpdate(){}
    }
}