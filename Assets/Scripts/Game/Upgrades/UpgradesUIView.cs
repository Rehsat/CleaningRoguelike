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
        private UpgradeDataView _upgradeDataViewPrefab;
        private List<UpgradeDataView> _activeUpgradeDataViews;
        public IReadOnlyReactiveTrigger OnUpgradesReset { get; }
        public IReadOnlyReactiveEvent<UpgradeData> OnTryBuyUpgrade { get; }
        [Inject]
        public void Construct(PrefabsContainer prefabsContainer)
        {
            _upgradeDataViewPrefab = prefabsContainer.GetPrefabsComponent<UpgradeDataView>(Prefab.UpgradeView);
            _activeUpgradeDataViews = new List<UpgradeDataView>();

        }
        public void SetUpgrades(List<UpgradeData> upgrades)
        {
            _upgradesRoot.enabled = true;
            for (var index = 0; index < upgrades.Count; index++)
            {
                var upgradeData = upgrades[index];
                var upgradeView = Instantiate( _upgradeDataViewPrefab, _upgradesRoot.transform);
                _activeUpgradeDataViews.Add(upgradeView);
            }

            Observable.TimerFrame(1).Subscribe((l =>
            {
                _upgradesRoot.enabled = false;
            }));
        }

        public void SetUpgradesResetCost(float cost)
        {
        }
    }
}