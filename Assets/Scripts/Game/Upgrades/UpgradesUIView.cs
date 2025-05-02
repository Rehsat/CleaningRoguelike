using System.Collections.Generic;
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
        [SerializeField] private LayoutGroup _upgradesRoot;
        private CompositeDisposable _upgradeViewsDisposable;
        private UpgradeDataView _upgradeDataViewPrefab;
        private ReactiveEvent<UpgradeData> _onTryBuyUpgrade;
        private List<UpgradeDataView> _activeUpgradeDataViews;
        public IReadOnlyReactiveTrigger OnUpgradesReset { get; }
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
            
        }

        public void SetUpgradesResetCost(float cost)
        {
        }

        private void SpawnUpgradesView(List<UpgradeData> upgrades)
        {
            _upgradeViewsDisposable?.Dispose();
            _upgradeViewsDisposable = new CompositeDisposable();
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
    }
}