using System;
using UniRx;
using Zenject;

namespace Game.Quota
{
    public class QuotaCostManager :IProgressModelContainer
    {
        private readonly PlayerGameValueData _playerPlayerGameValues;
        private ReactiveProperty<int> _currentQuotaIteration;

        public IReadOnlyReactiveProperty<float> CurrentProgress => _playerPlayerGameValues.CurrentValue;
        public IReadOnlyReactiveProperty<float> GoalProgress => _playerPlayerGameValues.MaxValue;
        
        [Inject]
        public QuotaCostManager(GameValuesContainer gameValuesContainer)
        {
            _playerPlayerGameValues = gameValuesContainer.GetPlayerValue(PlayerValue.QuotaMoney);
            _currentQuotaIteration = new ReactiveProperty<int>();
            _currentQuotaIteration.Subscribe(currentIteration =>
            {
                var newMaxValue = (float)Math.Pow(2, 5 + currentIteration);
                newMaxValue = newMaxValue.RoundToValue(10);
                _playerPlayerGameValues.SetCurrentValue(0);
                _playerPlayerGameValues.SetMaxValue(newMaxValue);
            });
        }

        public void ChangeQuotaIterationBy(int value)
        {
            _currentQuotaIteration.Value = Math.Clamp(_currentQuotaIteration.Value + value, 0, Int32.MaxValue);
        }
    }
}
