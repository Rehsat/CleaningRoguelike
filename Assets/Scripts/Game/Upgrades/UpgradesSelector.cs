using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using Game.Configs;
using Game.GameStateMachine;
using UniRx;
using UnityEngine;

namespace Game.Upgrades
{
    public class UpgradesSelector
    {
        private readonly GameGlobalConfig _config;
        private List<UpgradeConfig> _availableUpgrades;
        private ReactiveProperty<List<UpgradeData>> _currentUpgrades;

        public IReadOnlyReactiveProperty<List<UpgradeData>> CurrentUpgrades => _currentUpgrades;

        public UpgradesSelector(GameGlobalConfig config)
        {
            _config = config;
            _availableUpgrades = new List<UpgradeConfig>();
            _currentUpgrades = new ReactiveProperty<List<UpgradeData>>(new List<UpgradeData>());
            
            config.StartPossibleConfigs.ForEach
                (_availableUpgrades.Add);
        }

        public void SelectNewUpgrades(int upgradesCount)
        {
            var upgradesToSelectFrom = new List<UpgradeData>();
            _availableUpgrades.ForEach
                (availableUpgrade => upgradesToSelectFrom.Add(availableUpgrade.ConvertToData()));
            
            var selectedUpgrades = new List<UpgradeData>();
            for (int i = 0; i < upgradesCount; i++)
            {
                if(upgradesToSelectFrom.Count <= 0)
                    continue;

                var newSelectedUpgradeIndex = Random.Range(0, upgradesToSelectFrom.Count);
                var newSelectedUpgrade = upgradesToSelectFrom[newSelectedUpgradeIndex];
                selectedUpgrades.Add(newSelectedUpgrade);

                upgradesToSelectFrom.RemoveAt(newSelectedUpgradeIndex);
            }

            _currentUpgrades.Value = selectedUpgrades;
        }
    }
}
