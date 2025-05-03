using System;
using System.Collections.Generic;
using System.Linq;
using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using Gasme.Configs;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Upgrades
{
    public class UpgradesUIView : MonoBehaviour, IUpgradeView
    {
        [SerializeField] private Button _rerollButton;
        [SerializeField] private LayoutGroup _upgradesRoot;
        private CompositeDisposable _upgradeViewsDisposable;
        private UpgradeDataView _upgradeDataViewPrefab;
        private List<UpgradeDataView> _activeUpgradeDataViews;

        private ReactiveTrigger _onTryResetUpgrades;
        private ReactiveEvent<UpgradeData> _onTryBuyUpgrade;
        public IReadOnlyReactiveTrigger OnUpgradesReroll {
            get
            {
                if(_onTryResetUpgrades == null)
                    _onTryResetUpgrades = new ReactiveTrigger();
                return _onTryResetUpgrades;
            }
        }
        public IReadOnlyReactiveEvent<UpgradeData> OnTryBuyUpgrade
        {
            get
            {
                if(_onTryBuyUpgrade == null)
                    _onTryBuyUpgrade = new ReactiveEvent<UpgradeData>();
                return _onTryBuyUpgrade;
            }
        }

        [Inject]
        public void Construct(PrefabsContainer prefabsContainer)
        {
            _upgradeDataViewPrefab = prefabsContainer.GetPrefabsComponent<UpgradeDataView>(Prefab.UpgradeView);
            _activeUpgradeDataViews = new List<UpgradeDataView>();
            _rerollButton.onClick.AddListener(SendResetCallback);
        }
        public void SetUpgrades(List<UpgradeData> upgrades)
        {
            _upgradesRoot.enabled = true;
            SpawnUpgradesView(upgrades);
            
            Observable.TimerFrame(1).Subscribe((l =>
            {
                _upgradesRoot.enabled = false;
            }));
        }

        public void ApplyUpgradeCallback(UpgradeData upgradeData, bool buySuccess)
        {
            var upgradeView = _activeUpgradeDataViews
                .Find(view => view.UpgradeHashCode == upgradeData.GetHashCode());

            if (buySuccess)
            {
                RemoveUpgradeView(upgradeView);
            }
        }

        private void RemoveUpgradeView(UpgradeDataView upgradeView)
        {
            _activeUpgradeDataViews.Remove(upgradeView);
            Destroy(upgradeView.gameObject);
        }
        public void SetUpgradesRerollCost(float cost)
        {
        }

        private void SpawnUpgradesView(List<UpgradeData> upgrades)
        {
            ClearUpgradeViews();
            for (var index = 0; index < upgrades.Count; index++)
            {
                var upgradeData = upgrades[index];
                var upgradeView = Instantiate( _upgradeDataViewPrefab, _upgradesRoot.transform);
                upgradeView.Construct(upgradeData);
                
                upgradeView.OnBuy
                    .SubscribeWithSkip(_onTryBuyUpgrade.Notify)
                    .AddTo(_upgradeViewsDisposable);
                
                _activeUpgradeDataViews.Add(upgradeView);
            }
        }

        private void ClearUpgradeViews()
        {
            _upgradeViewsDisposable?.Dispose();
            _upgradeViewsDisposable = new CompositeDisposable();
            _activeUpgradeDataViews?.ToList().ForEach(RemoveUpgradeView);
        }

        private void SendResetCallback()
        {
            _onTryResetUpgrades.Notify();
        }

        private void OnDestroy()
        {
            _rerollButton.onClick.RemoveListener(SendResetCallback);
        }
    }
}