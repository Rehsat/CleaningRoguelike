using System;
using DG.Tweening;
using EasyFramework.ReactiveEvents;
using UniRx;

namespace Game.Interactables
{
    public class PressingStateSwitchAction<TAction> : IWorkAction where TAction : IAction // seems like this class is for OnLookAction only
    {
        private readonly TAction _actionOnComplete;

        private CompositeDisposable _compositeDisposable;
        private Sequence _currentSequence;
        private InteractableView _currentInteractableView;

        private ReactiveProperty<float> _currentPresses;
        private ReactiveProperty<float> _pressesToComplete;
        private ReactiveProperty<bool> _isPressingEnabled;
        private ReactiveEvent<WorkState> _onWorkStateChange;
        public IReadOnlyReactiveEvent<WorkState> OnWorkStateChanged => _onWorkStateChange;

        public IReadOnlyReactiveProperty<float> CurrentProgress => _currentPresses;
        public IReadOnlyReactiveProperty<float> GoalProgress => _pressesToComplete;
        public PressingStateSwitchAction(TAction actionOnComplete, int pressesToComplete)
        {
            _actionOnComplete = actionOnComplete;
            _isPressingEnabled= new ReactiveProperty<bool>();
            _onWorkStateChange =new ReactiveEvent<WorkState>();
            _currentPresses = new ReactiveProperty<float>();  
            _pressesToComplete = new ReactiveProperty<float>(pressesToComplete);
            
            _isPressingEnabled.Subscribe((isEnabled =>
            {
                if(isEnabled)
                {
                    _compositeDisposable = new CompositeDisposable();
                    var time = 0.25f;
                    _currentSequence = Utils.GetWorkTween(_currentInteractableView.transform, time, Ease.Unset);
                    _currentSequence.Play();
                    
                    Observable.Interval(TimeSpan.FromSeconds(time)).Subscribe(f =>
                    {
                        _currentPresses.Value++;
                        if (_currentPresses.Value >= _pressesToComplete.Value) Complete();
                    }).AddTo(_compositeDisposable);
                    _onWorkStateChange.Notify(WorkState.Started);
                }
                else
                {
                    Stop();
                }

            }));
        }

        private void Stop()
        {
            _compositeDisposable?.Dispose();
            _currentSequence?.Kill();
            _onWorkStateChange.Notify(WorkState.Stopped);
        }

        private void Complete()
        {
            _isPressingEnabled.Value = false;
            _currentPresses.Value = 0;
            _actionOnComplete.ApplyAction(new ContextContainer());
            _onWorkStateChange.Notify(WorkState.Completed);
        }
        public void ApplyAction(ContextContainer context)
        {
            InteractedObjectContext currentInteractedObject = null;
            if(context.TryGetContext(out currentInteractedObject) == false) return;
            
            _currentInteractableView = currentInteractedObject.InteractableView;
            _isPressingEnabled.Value = !_isPressingEnabled.Value;
        }
    }
}