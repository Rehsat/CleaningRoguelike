using Game.Interactables.Contexts;
using UniRx;

namespace Game.Interactables
{
    public class ChangeSellablePrice : IAction
    {
        private readonly ReactiveProperty<float> _changeValue;
        public ReactiveProperty<float> ChangeValue => _changeValue;

        public ChangeSellablePrice(float changeValue)
        {
            _changeValue = new ReactiveProperty<float>(changeValue);
        }
        public void ApplyAction(ContextContainer context)
        {
            SellableContext sellableContext = null;
            if (context.TryGetContext(out sellableContext))
            {
                sellableContext.ChangeCostBy(_changeValue.Value);
            }
        }
    }
}