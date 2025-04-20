using System;
using DG.Tweening;
using EasyFramework.ReactiveEvents;
using Game.Clothing;
using Game.Interactables.Contexts;
using Game.Player.Data;
using UniRx;
using UnityEngine;

namespace Game.Interactables
{
    //принял решение хранить все Action в одном фале потому что пока так удобнее. Если будут проблемы - без проблем разделю на несколько
    
    public class DoSimpleAction : IAction
    {
        private readonly Action _action;

        public DoSimpleAction(Action action)
        {
            _action = action;
        }
        public void ApplyAction(ContextContainer context)
        {
            _action.Invoke();
        }
    }
    
    public class PickUpAction : IAction
    {
        public void ApplyAction(ContextContainer contextContainer)
        {
            //TODO ПОдумать как сделать читабельней
            if (contextContainer.TryGetContext<InteractedObjectContext>(out var pickUpView) &&
                contextContainer.TryGetContext<ObjectHolderContext>(out var objectHolder))
            {
                if (pickUpView.InteractableView.TryGetComponent<Rigidbody>(out var rigidbody))
                    objectHolder.ObjectHolder.TryPickUpObject(rigidbody);
                else
                    Debug.LogError("TRIED TO PICK UP OBJECT WITHOUT RIGIDBODY");
            }
        }
    }

    public class TimedAction: IProgressModelContainer, IWorkAction //where TAction : IAction
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
    public class ChangeClothingStateAction : IAction
    {
        private readonly ClothingChangerConfig _clothingChangerConfig;
        private readonly Transform _dropPosition;
        private readonly Vector3 _dropDirection;
        private ClothingContext _currentClothingContext;

        public bool HasClothing => _currentClothingContext != null;

        public ChangeClothingStateAction(ClothingChangerConfig clothingChangerConfig, Transform dropPosition, Vector3 dropDirection)
        {
            _clothingChangerConfig = clothingChangerConfig;
            _dropPosition = dropPosition;
            _dropDirection = dropDirection;
        }
        public void ApplyAction(ContextContainer context)
        {
            if (_currentClothingContext != null)
            {
                var clothing = _currentClothingContext.Clothing;
                
                clothing.SetCurrentClothingStage(_clothingChangerConfig.ResultStage, _clothingChangerConfig.ResultMesh);
                
                var clothingGameObject = clothing.gameObject;
                clothingGameObject.transform.position = _dropPosition.position;
                clothingGameObject.SetActive(true);
                
                var dropForce = _dropDirection * _clothingChangerConfig.DropSpeed * 0.3f +
                                Vector3.up * _clothingChangerConfig.DropSpeed;
                clothing.Rigidbody.velocity = Vector3.zero;
                clothing.Rigidbody.AddForce(dropForce, ForceMode.Impulse);

                _currentClothingContext = null;
                return;
            }
            
            if (context.TryGetContext(out _currentClothingContext))
            {
                var clothing = _currentClothingContext.Clothing;
                clothing.gameObject.SetActive(false);
                return;
            }
        }
    }

    public class ChangeSellablePrice : IAction
    {
        private readonly float _changeValue;

        public ChangeSellablePrice(float changeValue)
        {
            _changeValue = changeValue;
        }
        public void ApplyAction(ContextContainer context)
        {
            SellableContext sellableContext = null;
            if (context.TryGetContext(out sellableContext))
            {
                sellableContext.ChangeCostBy(_changeValue);
            }
        }
    }

    public enum WorkState
    {
        Stopped = 0,
        Started = 1,
        Completed = 2
    }
}