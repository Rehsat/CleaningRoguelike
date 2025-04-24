using System.Collections.Generic;
using UnityEngine;

namespace Game.Upgrades
{
    [CreateAssetMenu(menuName = "GameConfigs/UpgradeConfig", fileName = "UpgradeConfig")]
    public class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private UpgradeConfigData _config;
        [SerializeField] private List<ActionConfig> _actionsOnApply;
        public UpgradeData ConvertToData()
        {
            var data = new UpgradeData(_config);
            _actionsOnApply.ForEach(config => data.AddAction(config.ConvertToAction()));
            return data;
        }
    }
}