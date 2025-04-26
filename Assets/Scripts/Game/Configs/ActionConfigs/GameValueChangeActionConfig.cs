using Game.Interactables;
using Game.Interactables.Contexts;
using Game.Upgrades;
using UnityEngine;

namespace Game.Configs.ActionConfigs
{
    public class GameValueChangeActionConfig : ActionConfig
    {
        [SerializeField] private Resource _valueType;
        [SerializeField] private float _change;

        public override IAction ConvertToAction()
        {
            return new GameValueChangeAction(_valueType, _change);
        }
    }
}
