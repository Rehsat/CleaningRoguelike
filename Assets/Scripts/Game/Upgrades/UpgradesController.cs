using System;
using Game.Interactables;
using Game.Interactables.Contexts;
using Zenject;
using UniRx;
using UnityEngine;

namespace Game.Upgrades
{
    //В теории можно разделить Reroll и Upgrade, но не вижу в этом особо смыла
    public class UpgradesController : IDisposable, IGlobalContextListener
    {
        private readonly GameValuesContainer _gameValuesContainer;
        private UpgradesSelector _upgradesSelector;
        private IUpgradeView _upgradeView;
        private ContextContainer _globalContextContainer;
        private CompositeDisposable _compositeDisposable;

        private int _lastCountOfUpgrades;
        [Inject]
        private UpgradesController(UpgradesSelector upgradesSelector,
         IUpgradeView upgradeView, 
         GameValuesContainer gameValuesContainer)
        {
            _compositeDisposable = new CompositeDisposable();
            _upgradeView = upgradeView;
            _gameValuesContainer = gameValuesContainer;
            _upgradesSelector = upgradesSelector;
            
            _upgradesSelector.CurrentUpgrades.Subscribe(upgradeView.SetUpgrades).AddTo(_compositeDisposable);
            _upgradeView.OnTryBuyUpgrade.SubscribeWithSkip(TryBuyUpgrade).AddTo(_compositeDisposable);
            _upgradeView.OnUpgradesReroll.SubscribeWithSkip(TryRerollUpgrades).AddTo(_compositeDisposable);
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
            var upgradeMoneyResource = _gameValuesContainer.GetPlayerValue(PlayerValue.UpgradeMoney);
            var currentUpgradeMoney = upgradeMoneyResource.CurrentValue;
            var isBuySuccess = currentUpgradeMoney.Value >= upgradeData.Cost;
            if (isBuySuccess)
            {
                upgradeMoneyResource.ChangeValueBy(upgradeData.ConfigData.Cost);
                upgradeData.ApplyActions(_globalContextContainer);
            }
            
            _upgradeView.ApplyUpgradeCallback(upgradeData, isBuySuccess);
        }

        private void TryRerollUpgrades()
        {
            _upgradesSelector.SelectNewUpgrades(_lastCountOfUpgrades);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}
