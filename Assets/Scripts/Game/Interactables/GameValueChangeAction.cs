using Game.Interactables.Contexts;

namespace Game.Interactables
{
    public class GameValueChangeAction : IAction
    {
        private readonly PlayerValue _valueType;
        private readonly float _changeBy;
        private readonly bool _maxValue;

        public GameValueChangeAction(PlayerValue valueType, float changeBy, bool maxValue)
        {
            _valueType = valueType;
            _changeBy = changeBy;
            _maxValue = maxValue;
        }
        public void ApplyAction(ContextContainer context)
        {
            if (context.TryGetContext<GameValuesContext>(out var gameValues))
            {
                var resource =  gameValues.GameResources.GetPlayerValue(_valueType);

                if (_maxValue)
                    resource.ChangeMaxValueBy(_changeBy);
                else
                    resource.ChangeValueBy(_changeBy);

            }
        }
    }
}