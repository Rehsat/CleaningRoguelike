using System;
using Game.Interactables;
using Game.Interactables.Contexts;
using Zenject;
using UniRx;
using UnityEngine;

namespace Game.Upgrades
{
    public class UpgradeController : IDisposable, IGlobalContextListener
    {
        private UpgradesSelector _upgradesSelector;
        private IUpgradeView _upgradeView;
        private ContextContainer _globalContextContainer;
        private CompositeDisposable _compositeDisposable;

        private int _lastCountOfUpgrades;
        [Inject]
     /*   public UpgradeController(UpgradesSelector upgradesSelector, SceneObjectsContainer sceneObjectsContainer)
        {
            Debug.LogError(GetHashCode());
            var upgradeView = sceneObjectsContainer.GetObjectsComponent<IUpgradeView>(SceneObject.UpgradeView);
            Init(upgradesSelector, upgradeView);
        }*/
        private UpgradeController(UpgradesSelector upgradesSelector, IUpgradeView upgradeView)
        {
            _compositeDisposable = new CompositeDisposable();
            _upgradeView = upgradeView;
            _upgradesSelector = upgradesSelector;
            
            _upgradesSelector.CurrentUpgrades.Subscribe(upgradeView.SetUpgrades).AddTo(_compositeDisposable);
            _upgradeView.OnTryBuyUpgrade.SubscribeWithSkip(TryBuyUpgrade).AddTo(_compositeDisposable);
            _upgradeView.OnUpgradesReroll.SubscribeWithSkip(TryResetReroll).AddTo(_compositeDisposable);
        }
        public void SelectNewUpgrades(int upgradesCount)
        {
            _lastCountOfUpgrades = upgradesCount;
            _upgradesSelector.SelectNewUpgrades(upgradesCount);
        }

        public void SetContext(GlobalContextContainer globalContextContainer)
        {
            _globalContextContainer = globalContextContainer.ContextContainer;
        }
        private void TryBuyUpgrade(UpgradeData upgradeData)
        {
            upgradeData.ApplyActions(_globalContextContainer);
            _upgradeView.ApplyUpgradeCallback(upgradeData, true);
        }

        private void TryResetReroll()
        {
            _upgradesSelector.SelectNewUpgrades(_lastCountOfUpgrades);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}
