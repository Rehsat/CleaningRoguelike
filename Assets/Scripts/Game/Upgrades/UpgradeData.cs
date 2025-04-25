using System.Collections.Generic;
using Game.Interactables;
using UnityEngine;

namespace Game.Upgrades
{
    public class UpgradeData
    {
        private readonly UpgradeConfigData _configData;
        private List<IAction> _upgradeActions;

        public UpgradeData(UpgradeConfigData configData)
        {
            _upgradeActions = new List<IAction>();
            _configData = configData;
        }

        public UpgradeData AddAction(IAction action)
        {
            _upgradeActions.Add(action);
            return this;
        }
    }

    public abstract class ActionConfig : ScriptableObject
    {
        public abstract IAction ConvertToAction();
    }
}
