using Game.Interactables.Contexts;
using Game.Upgrades;

namespace Game.GameStateMachine
{
    public class BootstrapState : ILevelState
    {
        private readonly SceneObjectsContainer _sceneObjectsContainer;
        private readonly CurrentGameStateObserver _currentGameStateObserver;
        private GameStateMachine _gameStateMachine;
        
        public BootstrapState(UpgradesController upgradesController, 
            SceneObjectsContainer sceneObjectsContainer, 
            GlobalContextContainer globalContextContainer
            ,CurrentGameStateObserver currentGameStateObserver
        )
        {
            _sceneObjectsContainer = sceneObjectsContainer;
            _currentGameStateObserver = currentGameStateObserver;
        }
        public void Enter()
        {
            _currentGameStateObserver.SetStateMachine(_gameStateMachine);
            _sceneObjectsContainer.SetupGameStateObserver(_currentGameStateObserver);
            _gameStateMachine.EnterState<UpgradeGameState>();
        }

        public void Exit()
        {
        }


        public void SetStateMachine(GameStateMachine stateMachine)
        {
            _gameStateMachine = stateMachine;
        }
        public void OnUpdate(){}
    }
}