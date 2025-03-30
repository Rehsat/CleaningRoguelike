using UniRx;

namespace Game.Interactables.Contexts
{
    public class SellableContext : IInteractableContext
    {
        private ReactiveProperty<float> _cost;
        public IReadOnlyReactiveProperty<float> Cost => _cost;

        public SellableContext(float startCost = 0)
        {
            _cost = new ReactiveProperty<float>(startCost);
        }

        public void ChangeCostBy(float value)
        {
            _cost.Value += value;
        }
    }
}