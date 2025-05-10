using Game.Clothing;
using Game.Interactables;
using Game.Interactables.Actions;
using Game.Upgrades;
using UnityEngine;

namespace Game.Configs.ActionConfigs
{
    [CreateAssetMenu(menuName = "GameConfigs/ActionConfigs/SpawnWashingMachineActionConfig", fileName ="SpawnWashingMachineActionConfig")]
    public class SpawnWashingMachineActionConfig : ActionConfig
    {
        [SerializeField] private ClothingChangerConfig _config;
        public override IAction ConvertToAction()
        {
            return new SpawnWashingMachineAction(_config);
        }
    }
}