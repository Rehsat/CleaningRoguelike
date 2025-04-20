using System;
using Game.Interactables;
using Game.Quota;
using Game.UI.Interactables;
using UniRx;

namespace Game.GameStateMachine
{
    public class DoWorkState : ILevelState
    {
        private readonly QuotaCostManager _quotaCostManager;
        private CompositeDisposable _compositeDisposable;
        private GameStateMachine _gameStateMachine;
        private TimedAction _quotaTimerAction;

        public DoWorkState(SceneObjectsContainer sceneObjectsContainer, QuotaCostManager quotaCostManager)
        {
            _quotaCostManager = quotaCostManager;
            var onTimerCompleteAction = new DoSimpleAction(OnQuotaTimerComplete);
            var quotaTimeProgressBar = sceneObjectsContainer.GetObjectsComponent<ProgressBarView>(SceneObject.QuotaTimeProgressBar);
            _quotaTimerAction = new TimedAction(onTimerCompleteAction, 35);
            
            var quotaTimerProgressPresenter = new ProgressPresenter(_quotaTimerAction, quotaTimeProgressBar);
            
        }
        public void SetStateMachine(GameStateMachine stateMachine)
        {
            _gameStateMachine = stateMachine;
        }
        public void Enter()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = new CompositeDisposable();
            _quotaTimerAction.ApplyAction(new ContextContainer());
        }

        public void Exit()
        {
            _compositeDisposable?.Dispose();
        }

        public void OnUpdate(){}

        private void OnQuotaTimerComplete()
        {
            _gameStateMachine.EnterState<UpgradeState>();
            _quotaCostManager.ChangeQuotaIterationBy(1);
        }
    }
}