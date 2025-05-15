using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameStateMachine;
using UniRx;
using UnityEngine;

public class CurrentGameStateObserver 
{
    private ReactiveProperty<GameState> _currentGameState;
    private Dictionary<Type, GameState> _gameStateByType;
    public ReactiveProperty<GameState> CurrentGameState => _currentGameState;

    public CurrentGameStateObserver()
    {
        _currentGameState = new ReactiveProperty<GameState>();
        _gameStateByType = new Dictionary<Type, GameState>()
        {
            {typeof(UpgradeGameState), GameState.Upgrade},
            {typeof(DoWorkState), GameState.Work}
        };
    }

    public void SetStateMachine(GameStateMachine gameStateMachine)
    {
        gameStateMachine.OnStateChange.SubscribeWithSkip(currentState =>
        {
            var currentStateType = currentState.GetType();
            var gameState = GameState.Unknown;

            if (_gameStateByType.ContainsKey(currentStateType))
                gameState = _gameStateByType[currentStateType];
            _currentGameState.Value = gameState;
        });
    }
    
}

public enum GameState
{
    Unknown = 0,
    Upgrade = 1,
    Work = 2
}
