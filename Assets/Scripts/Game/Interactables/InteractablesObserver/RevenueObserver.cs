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
            ChangeSellablePrice priceChangeAction;
            if (_interactableView.TryGetAction(out priceChangeAction))
            {
                var revenue = priceChangeAction.ChangeValue;
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
