using Game.Interactables;
using Game.Interactables.Contexts;
using Game.Upgrades;
using UniRx;
using Zenject;

namespace Game.GameStateMachine
{
    public class UpgradeGameState : ILevelState
    {
        private readonly UpgradesController _upgradesController;
        private readonly InteractableButton _startQuotaButton;

        private CompositeDisposable _compositeDisposable;
        private GameStateMachine _stateMachine;

        [Inject]
        public UpgradeGameState(SceneObjectsContainer sceneObjects, UpgradesController upgradesController)
        {
            _upgradesController = upgradesController;
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
            
            _upgradesController.SelectNewUpgrades(3);
            
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
        private GameStateMachine _gameStateMachine;
        public BootstrapState(UpgradesController upgradesController, 
            SceneObjectsContainer sceneObjectsContainer, 
            GlobalContextContainer globalContextContainer)
        {
            
            _gameStateMachine.EnterState<UpgradeGameState>();
        }
        public void Enter()
        {
            
        }

        public void Exit()
        {
        }


        public void SetStateMachine(GameStateMachine stateMachine)
        {
            _gameStateMachine = stateMachine;
        }
        public void OnUpdate(){}
    }
}