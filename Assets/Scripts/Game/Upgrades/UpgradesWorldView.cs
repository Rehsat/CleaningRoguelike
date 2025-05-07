using System;
using System.Collections.Generic;
using DG.Tweening;
using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using Game.Player.PayerInput;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Upgrades
{
    public class UpgradesWorldView : MonoBehaviour, IUpgradeView
    {
        [SerializeField] private UpgradesUIView _upgradesUIView;
        [SerializeField] private Transform _rootTransform;
        [SerializeField] private float _showRotation = 90;
        [SerializeField] private float _hideRotation = -20;
        [SerializeField] private float _animationDuration = 0.5f;
        private bool _isAnimationInProgress;
        private CompositeDisposable _compositeDisposable;
        
        public IReadOnlyReactiveTrigger OnUpgradesReroll => _upgradesUIView.OnUpgradesReroll;
        public IReadOnlyReactiveEvent<UpgradeData> OnTryBuyUpgrade => _upgradesUIView.OnTryBuyUpgrade;
        
        [Inject]
        public void Construct()
        {
            _compositeDisposable = new CompositeDisposable();
        }

        public void SetShowState(bool isShowing)
        {
            _isAnimationInProgress = true;
            
            var resultRotation = isShowing ? _showRotation : _hideRotation;
            var startRotation = isShowing ? _hideRotation : _showRotation;
            var ease = isShowing ? Ease.OutBack : Ease.InBack;
                
         //   _rootTransform
         //       .DOLocalRotate(new Vector3(startRotation, _rootTransform.localEulerAngles.y), 0);
            _rootTransform
                .DOLocalRotate(new Vector3(resultRotation, _rootTransform.localRotation.y), _animationDuration)
                .SetEase(ease)
                .OnComplete((() => _isAnimationInProgress = false));
        }
        public void SetUpgrades(List<UpgradeData> upgrades)
        {
            _upgradesUIView.SetUpgrades(upgrades);
        }

        public void ApplyUpgradeCallback(UpgradeData upgradeData, bool buySuccess)
        {
            _upgradesUIView.ApplyUpgradeCallback(upgradeData, buySuccess);
        }

        public void SetUpgradesRerollCost(float cost)
        {
            _upgradesUIView.SetUpgradesRerollCost(cost);
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }
    }
}
