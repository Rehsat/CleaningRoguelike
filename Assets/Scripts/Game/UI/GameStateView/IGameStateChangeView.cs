namespace Game.UI
{
    public interface IGameStateChangeView
    {
        public void OnGameStateChanged(GameState currentState);
    }
}