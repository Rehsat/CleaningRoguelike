﻿using System;
using DG.Tweening;
using EasyFramework.ReactiveEvents;
using Game.Clothing;
using Game.Interactables.Contexts;
using Game.Player.Data;
using UniRx;
using UnityEngine;

namespace Game.Interactables
{
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

    public class TimedAction<TAction> : IWorkAction where TAction : IAction
    {
        private readonly TAction _action;
        private readonly float _secondsToComplete;

        private ReactiveEvent<bool> _onWorkStateChanged;
        private ReactiveProperty<float> _timer;
        private CompositeDisposable _compositeDisposable;
        private Sequence _sequence;

        public IReadOnlyReactiveEvent<bool> OnWorkStateChanged => _onWorkStateChanged;
        public TimedAction(TAction action, float secondsToComplete)
        {
            _action = action;
            _onWorkStateChanged = new ReactiveEvent<bool>();
            _secondsToComplete = secondsToComplete;
            _timer = new ReactiveProperty<float>(secondsToComplete);
        }

        public void ApplyAction(ContextContainer context)
        {
            _compositeDisposable = new CompositeDisposable();
            _sequence = context.TryGetContext<InteractedObjectContext>(out var interactedObject)
                ? Utils.GetWorkTween(interactedObject.InteractableView.transform)
                : null;
            Observable.EveryUpdate().Subscribe((l =>
            {
                _timer.Value -= Time.deltaTime;
                if(_timer.Value <=0) CompleteAction(context);
            })).AddTo(_compositeDisposable);
            
            _onWorkStateChanged.Notify(true);
        }

        private void CompleteAction(ContextContainer context)
        {
            _sequence?.Kill();
            _compositeDisposable.Dispose();
            _timer.Value = _secondsToComplete;
            Debug.LogError(_timer.Value);
            _action.ApplyAction(context); 
            
            _onWorkStateChanged.Notify(false);
        }
    }

    public class PressingStateSwitchAction<TAction> : IWorkAction where TAction : IAction // seems like this class is for OnLookAction only
    {
        private readonly TAction _actionOnComplete;

        private CompositeDisposable _compositeDisposable;
        private Sequence _currentSequence;
        private InteractableView _currentInteractableView;

        private ReactiveProperty<int> _currentPresses;
        private ReactiveProperty<bool> _isPressingEnabled;
        private ReactiveEvent<bool> _onWorkCompleteStateChange;
        public IReadOnlyReactiveEvent<bool> OnWorkStateChanged => _onWorkCompleteStateChange;
        public PressingStateSwitchAction(TAction actionOnComplete, int pressesToComplete)
        {
            _actionOnComplete = actionOnComplete;
            _isPressingEnabled= new ReactiveProperty<bool>();
            _onWorkCompleteStateChange =new ReactiveEvent<bool>();
            _currentPresses = new ReactiveProperty<int>();  
            
            _isPressingEnabled.Subscribe((isEnabled =>
            {
                if(isEnabled)
                {
                    _onWorkCompleteStateChange.Notify(true);
                    _compositeDisposable = new CompositeDisposable();
                    var time = 0.2f;
                    _currentSequence = Utils.GetWorkTween(_currentInteractableView.transform, time, Ease.Unset);
                    _currentSequence.Play();
                    
                    Observable.Interval(TimeSpan.FromSeconds(time)).Subscribe(f =>
                    {
                        _currentPresses.Value++;
                        if (_currentPresses.Value >= pressesToComplete) Complete();
                    }).AddTo(_compositeDisposable);
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
        }

        private void Complete()
        {
            _isPressingEnabled.Value = false;
            _currentPresses.Value = 0;
            _actionOnComplete.ApplyAction(new ContextContainer());
            _onWorkCompleteStateChange.Notify(false);
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
        private readonly ClothingChangerSettings _clothingChangerSettings;
        private readonly Vector3 _dropDirection;
        private ClothingContext _currentClothingContext;

        public bool HasClothing => _currentClothingContext != null;

        public ChangeClothingStateAction(ClothingChangerSettings clothingChangerSettings, Vector3 dropDirection)
        {
            _clothingChangerSettings = clothingChangerSettings;
            _dropDirection = dropDirection;
        }
        public void ApplyAction(ContextContainer context)
        {
            if (_currentClothingContext != null)
            {
                var clothing = _currentClothingContext.Clothing;
                
                clothing.SetCurrentClothingStage(_clothingChangerSettings.ResultStage, _clothingChangerSettings.ResultMesh);
                
                var clothingGameObject = clothing.gameObject;
                clothingGameObject.transform.position = _clothingChangerSettings.DropPosition.position;
                clothingGameObject.SetActive(true);
                
                var dropForce = _dropDirection * _clothingChangerSettings.DropSpeed * 0.3f +
                                Vector3.up * _clothingChangerSettings.DropSpeed;
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
}