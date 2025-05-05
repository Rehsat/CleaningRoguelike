using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;

namespace Game.Upgrades
{
    public interface IUpgradeView : IShowable
    {
        public IReadOnlyReactiveTrigger OnUpgradesReroll { get;}
        public IReadOnlyReactiveEvent<UpgradeData> OnTryBuyUpgrade { get; }
        public void SetUpgrades(List<UpgradeData> upgrades);
        public void ApplyUpgradeCallback(UpgradeData upgradeData, bool buySuccess);
        public void SetUpgradesRerollCost(float cost);
    }

    public interface IShowable
    {
        public void SetShowState(bool isShowed);
    }
}