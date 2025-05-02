using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;

namespace Game.Upgrades
{
    public interface IUpgradeView
    {
        public IReadOnlyReactiveTrigger OnUpgradesReset { get;}
        public IReadOnlyReactiveEvent<UpgradeData> OnTryBuyUpgrade { get; }
        public void SetUpgrades(List<UpgradeData> upgrades);
        public void ApplyUpgradeCallback(UpgradeData upgradeData, bool buySuccess);
        public void SetUpgradesResetCost(float cost);
    }
}