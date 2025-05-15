using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Interactables.InteractablesObserver
{
    public class RevenueObserver : MonoBehaviour
    {
        [SerializeField] private InteractableView _interactableView;
        [SerializeField] private TMP_Text _revenueText;
        private CompositeDisposable _compositeDisposable;

        public void Start()
        {
            _compositeDisposable = new CompositeDisposable();
            ChangeSellablePriceAction priceActionChangeAction;
            if (_interactableView.TryGetAction(typeof(ChangeSellablePriceAction),out var action))
            {
                priceActionChangeAction = (ChangeSellablePriceAction)action;
                var revenue = priceActionChangeAction.ChangeValue;
                revenue.Subscribe((value =>
                {
                    _revenueText.text = $"+{value}";
                })).AddTo(_compositeDisposable);
            }
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }
    }
}
