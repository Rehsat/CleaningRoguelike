using Game.Player.PayerInput;
using Game.Services;
using UniRx;

namespace Game.Upgrades
{
    public class UpgradesShowStateController : ICursorRequire
    {
        private readonly PlayerInput _playerInput;
        private readonly IUpgradeView _upgradeView;
        private CompositeDisposable _compositeDisposable;
        private ReactiveProperty<bool> _isUpgradesShowed;
        public IReadOnlyReactiveProperty<bool> IsCursorRequired => _isUpgradesShowed;

        public UpgradesShowStateController(PlayerInput playerInput, IUpgradeView upgradeView)
        {
            _playerInput = playerInput;
            _upgradeView = upgradeView;
            _compositeDisposable = new CompositeDisposable();
            _isUpgradesShowed = new ReactiveProperty<bool>();
            
            _playerInput.OnUpgradesOpenButtonPressed.SubscribeWithSkip((() =>
            {
                _isUpgradesShowed.Value = !_isUpgradesShowed.Value;
            })).AddTo(_compositeDisposable);
            InitShowSubscribe();
        }

        private void InitShowSubscribe()
        {
            var isFirstTime = true;
            _isUpgradesShowed.Subscribe((isShowing =>
            {
                if (isFirstTime)
                {
                    isFirstTime = false;
                    return;
                }
                
                _upgradeView.SetShowState(isShowing);
                
            })).AddTo(_compositeDisposable);
        }
    }
}