using System;
using UniRx;
using Zenject;

namespace Game.Quota
{
    public class QuotaCostManager :IProgressModelContainer
    {
        private readonly ResourceData _playerResources;
        private ReactiveProperty<int> _currentQuotaIteration;

        public IReadOnlyReactiveProperty<float> CurrentProgress => _playerResources.CurrentValue;
        public IReadOnlyReactiveProperty<float> GoalProgress => _playerResources.MaxValue;
        
        [Inject]
        public QuotaCostManager(PlayerResources playerResources)
        {
            _playerResources = playerResources.GetResource(Resource.QuotaMoney);
            _currentQuotaIteration = new ReactiveProperty<int>();
            _currentQuotaIteration.Subscribe(currentIteration =>
            {
                var newMaxValue = (float)Math.Pow(2, 5 + currentIteration);
                newMaxValue = newMaxValue.RoundToValue(10);
                _playerResources.SetCurrentValue(0);
                _playerResources.SetMaxValue(newMaxValue);
            });
        }

        public void ChangeQuotaIterationBy(int value)
        {
            _currentQuotaIteration.Value = Math.Clamp(_currentQuotaIteration.Value + value, 0, Int32.MaxValue);
        }
    }
}
