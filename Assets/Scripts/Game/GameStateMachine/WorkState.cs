using System;
using UniRx;

namespace Game.GameStateMachine
{
    public class WorkState : ILevelState
    {
        private CompositeDisposable _compositeDisposable;
        private GameStateMachine _gameStateMachine;
        public void SetStateMachine(GameStateMachine stateMachine)
        {
            _gameStateMachine = stateMachine;
        }
        public void Enter()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = new CompositeDisposable();
            Observable.Timer(TimeSpan.FromSeconds(5)).Subscribe((l =>
            {
                _gameStateMachine.EnterState<UpgradeState>();
            })).AddTo(_compositeDisposable);
        }

        public void Exit()
        {
            _compositeDisposable?.Dispose();
        }

        public void OnUpdate()
        {
        }
    }
}