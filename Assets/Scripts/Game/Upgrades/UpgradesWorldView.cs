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
        private ReactiveProperty<bool> _isUpgradeShowed;
        
        public IReadOnlyReactiveTrigger OnUpgradesReset => _upgradesUIView.OnUpgradesReset;
        public IReadOnlyReactiveEvent<UpgradeData> OnTryBuyUpgrade => _upgradesUIView.OnTryBuyUpgrade;
        
        [Inject]
        public void Construct(PlayerInput playerInput)
        {
            _isUpgradeShowed = new ReactiveProperty<bool>();
            playerInput.OnUpgradesOpenButtonPressed.SubscribeWithSkip((() =>
            {
                if (_isAnimationInProgress) return;
                _isUpgradeShowed.Value = !_isUpgradeShowed.Value;
            }));

            var isFirstTime = true;
            _isUpgradeShowed.Subscribe((isShowing =>
            {
                if (isFirstTime)
                {
                    isFirstTime = false;
                    return;
                }
                SetShowState(isShowing);
            }));
        }

        private void SetShowState(bool isShowing)
        {
            _isAnimationInProgress = true;
            
            var resultRotation = isShowing ? _showRotation : _hideRotation;
            var startRotation = isShowing ? _hideRotation : _showRotation;
            var ease = isShowing ? Ease.OutBack : Ease.InBack;
                
            _rootTransform
                .DOLocalRotate(new Vector3(startRotation, _rootTransform.localEulerAngles.y), 0);
            _rootTransform
                .DOLocalRotate(new Vector3(resultRotation, _rootTransform.localRotation.y), _animationDuration)
                .SetEase(ease)
                .OnComplete((() => _isAnimationInProgress = false));
        }
        public void SetUpgrades(List<UpgradeData> upgrades)
        {
            _upgradesUIView.SetUpgrades(upgrades);
        }

        public void SendUpgradeCallback(UpgradeData upgradeData, bool buySuccess)
        {
            _upgradesUIView.SendUpgradeCallback(upgradeData, buySuccess);
        }

        public void SetUpgradesResetCost(float cost)
        {
            _upgradesUIView.SetUpgradesResetCost(cost);
        }
    }
}
