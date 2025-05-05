using System.Collections.Generic;
using Game.Interactables;
using UniRx;
using UnityEngine;

namespace Game.Upgrades
{
    public class UpgradeData
    {
        private readonly UpgradeConfigData _configData;
        private List<IAction> _upgradeActions;
        private ReactiveProperty<bool> _isBought;

        public ReactiveProperty<bool> IsBought => _isBought;
        public UpgradeConfigData ConfigData => _configData;
        public float Cost => ConfigData.Cost;

        public UpgradeData(UpgradeConfigData configData)
        {
            _upgradeActions = new List<IAction>();
            _isBought = new ReactiveProperty<bool>();
            _configData = configData;
        }

        public UpgradeData AddAction(IAction action)
        {
            _upgradeActions.Add(action);
            return this;
        }

        public void SetIsBought(bool isBought)
        {
            _isBought.Value = isBought;
        }

        public void ApplyActions(ContextContainer context)
        {
            _upgradeActions.ForEach(action => action.ApplyAction(context));
        }
    }

    public abstract class ActionConfig : ScriptableObject
    {
        public abstract IAction ConvertToAction();
    }
}
