using Game.Interactables.Contexts;
using Game.Interactables.Stackable;
using UniRx;

namespace Game.Interactables
{
    public class ChangeSellablePriceAction : IAction, IStackable
    {
        private readonly ReactiveProperty<float> _changeValue;
        public ReactiveProperty<float> ChangeValue => _changeValue;

        public ChangeSellablePriceAction(float changeValue)
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

        public void Stack(IStackable objectToStackWith)
        {
            if (objectToStackWith is ChangeSellablePriceAction sellablePriceAction)
                _changeValue.Value += sellablePriceAction.ChangeValue.Value;
            
        }
    }
}