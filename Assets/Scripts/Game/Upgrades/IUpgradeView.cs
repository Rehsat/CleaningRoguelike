using System.Collections.Generic;
using EasyFramework.ReactiveTriggers;

namespace Game.Upgrades
{
    public interface IUpgradeView
    {
        public IReadOnlyReactiveTrigger OnUpgradesReset { get;}
        public void SetUpgrades(List<UpgradeData> upgrades);
        public void SetUpgradesResetCost(float cost);
    }
}