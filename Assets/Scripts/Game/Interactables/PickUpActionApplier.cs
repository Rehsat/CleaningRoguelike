using DG.Tweening;
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
        private ReactiveProperty<float> _timer;
        private CompositeDisposable _compositeDisposable;
        private Sequence _sequence;
        public TimedAction(TAction action, float secondsToComplete)
        {
            _action = action;
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
        }

        private void CompleteAction(ContextContainer context)
        {
            _sequence?.Kill();
            _compositeDisposable.Dispose();
            _timer.Value = _secondsToComplete;
            Debug.LogError(_timer.Value);
            _action.ApplyAction(context);
        }
    }
    
    public class ChangeClothingState : IAction
    {
        private readonly ClothingChangerSettings _clothingChangerSettings;

        public ChangeClothingState(ClothingChangerSettings clothingChangerSettings)
        {
            _clothingChangerSettings = clothingChangerSettings;
        }
        public void ApplyAction(ContextContainer context)
        {
            ClothingContext clothingContext;
            if (context.TryGetContext(out clothingContext))
            {
                var clothingGameObject = clothingContext.Clothing.gameObject;
                clothingGameObject.transform.position = _clothingChangerSettings.DropPosition.position;
                clothingGameObject.SetActive(true);
            }
        }
    }
}