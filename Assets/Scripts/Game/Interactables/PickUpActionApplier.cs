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

    public class TimedAction<TAction> : IAction where TAction : IAction
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

    public class PressingStateSwitchAction<TAction> : IAction where TAction : IAction // seems like this class is for OnLookAction only
    {

        private ReactiveProperty<bool> _isPressingEnabled;
        public PressingStateSwitchAction(TAction actionOnComplete, int pressesToComplete)
        {
            
        }
        public void ApplyAction(ContextContainer context)
        {
            _isPressingEnabled.Value = !_isPressingEnabled.Value;
        }
    }
    public class ChangeClothingStateAction : IAction
    {
        private readonly ClothingChangerSettings _clothingChangerSettings;
        private readonly Vector3 _dropDirection;

        public ChangeClothingStateAction(ClothingChangerSettings clothingChangerSettings, Vector3 dropDirection)
        {
            _clothingChangerSettings = clothingChangerSettings;
            _dropDirection = dropDirection;
        }
        public void ApplyAction(ContextContainer context)
        {
            ClothingContext clothingContext;
            if (context.TryGetContext(out clothingContext))
            {
                var clothing = clothingContext.Clothing;
                clothing.SetCurrentClothingStage(_clothingChangerSettings.ResultStage);
                
                var clothingGameObject = clothing.gameObject;
                clothingGameObject.transform.position = _clothingChangerSettings.DropPosition.position;
                clothingGameObject.SetActive(true);
                
                var dropForce = _dropDirection * _clothingChangerSettings.DropSpeed * 0.3f +
                                Vector3.up * _clothingChangerSettings.DropSpeed;
                
                clothing.Rigidbody.velocity = Vector3.zero;
                clothing.Rigidbody.AddForce(dropForce, ForceMode.Impulse);
            }
        }
    }
}