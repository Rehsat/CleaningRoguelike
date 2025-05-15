using Game.Interactables;
using Game.Upgrades;
using UniRx;
using Zenject;

namespace Game.GameStateMachine
{
    public class UpgradeGameState : ILevelState
    {
        private readonly UpgradesController _upgradesController;
        private readonly GameValuesContainer _gameValuesContainer;
        private readonly InteractableButton _startQuotaButton;

        private CompositeDisposable _compositeDisposable;
        private GameStateMachine _stateMachine;

        [Inject]
        public UpgradeGameState(SceneObjectsContainer sceneObjects
            ,UpgradesController upgradesController
            ,GameValuesContainer gameValuesContainer)
        {
            _upgradesController = upgradesController;
            _gameValuesContainer = gameValuesContainer;
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
            var income = _gameValuesContainer.GetPlayerValue(PlayerValue.Income);
            _gameValuesContainer.GetPlayerValue(PlayerValue.UpgradeMoney).ChangeValueBy(income.CurrentValue.Value + 15);
            
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
}