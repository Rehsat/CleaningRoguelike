using DG.Tweening;
using EasyFramework.ReactiveEvents;
using UniRx;
using UnityEngine;

namespace Game.Interactables
{
    public class TimedAction: IWorkAction //where TAction : IAction
    {
        private readonly IAction _action;

        private ReactiveEvent<WorkState> _onWorkStateChanged;
        private ReactiveProperty<float> _currentSecondsPassed;
        private ReactiveProperty<float> _secondsToComplete;
        private CompositeDisposable _compositeDisposable;
        private Sequence _sequence;

        public IReadOnlyReactiveProperty<float> CurrentProgress => _currentSecondsPassed;
        public IReadOnlyReactiveProperty<float> GoalProgress => _secondsToComplete;
        public IReadOnlyReactiveEvent<WorkState> OnWorkStateChanged => _onWorkStateChanged;
        public TimedAction(IAction action, float secondsToComplete)
        {
            _action = action;
            _onWorkStateChanged = new ReactiveEvent<WorkState>();
            _currentSecondsPassed = new ReactiveProperty<float>(0);
            _secondsToComplete =  new ReactiveProperty<float>(secondsToComplete);
        }

        public void ApplyAction(ContextContainer context)
        {
            _compositeDisposable = new CompositeDisposable();
            _currentSecondsPassed.Value = 0;
            _sequence = context.TryGetContext<InteractedObjectContext>(out var interactedObject)
                ? Utils.GetWorkTween(interactedObject.InteractableView.transform)
                : null;
            Observable.EveryUpdate().Subscribe((l =>
            {
                _currentSecondsPassed.Value += Time.deltaTime;
                if(_currentSecondsPassed.Value >= _secondsToComplete.Value) CompleteAction(context);
            })).AddTo(_compositeDisposable);
            
            _onWorkStateChanged.Notify(WorkState.Started);
        }

        private void CompleteAction(ContextContainer context)
        {
            _sequence?.Kill();
            _compositeDisposable.Dispose();
            _action.ApplyAction(context); 
            
            _onWorkStateChanged.Notify(WorkState.Completed);
        }

    }
}