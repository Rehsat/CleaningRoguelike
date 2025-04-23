using UniRx;

namespace Game.Player.Data
{
    public class StatData
    {
        private readonly Stat _type;
        private ReactiveProperty<float> _statValue;

        public ReactiveProperty<float> StatValue => _statValue;

        public StatData(Stat type, float startValue)
        {
            _type = type;
            _statValue = new ReactiveProperty<float>(startValue);
        }
    }
}