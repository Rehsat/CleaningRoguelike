using UniRx;

namespace Game.UI.GameStateView
{
    public class GameStateChangePresenter
    {
        public GameStateChangePresenter(CurrentGameStateObserver stateObserver, IGameStateChangeView view)
        {
            stateObserver.CurrentGameState.Subscribe(view.OnGameStateChanged);
        }
    }
}
