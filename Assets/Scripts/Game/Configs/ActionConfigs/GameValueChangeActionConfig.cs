using Game.Interactables;
using Game.Interactables.Contexts;
using Game.Upgrades;
using UnityEngine;

namespace Game.Configs.ActionConfigs
{
    [CreateAssetMenu(menuName = "GameConfigs/ActionConfigs/GameValueChangeActionConfig", fileName ="GameValueChangeActionConfig")]
    public class GameValueChangeActionConfig : ActionConfig
    {
        [SerializeField] private bool _maxValue;
        [SerializeField] private PlayerValue _valueType;
        [SerializeField] private float _change;

        public override IAction ConvertToAction()
        {
            return new GameValueChangeAction(_valueType, _change, _maxValue);
        }
    }
}
