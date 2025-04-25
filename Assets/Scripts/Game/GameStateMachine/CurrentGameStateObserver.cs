using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameStateMachine;
using UniRx;
using UnityEngine;

public class CurrentGameStateObserver 
{
    private ReactiveProperty<GameState> _currentGameState;
    public ReactiveProperty<GameState> CurrentGameState => _currentGameState;

    public CurrentGameStateObserver(GameStateMachine gameStateMachine)
    {
        _currentGameState = new ReactiveProperty<GameState>();
        var gameStateByType = new Dictionary<Type, GameState>()
        {
            {typeof(UpgradeGameState), GameState.Upgrade},
            {typeof(DoWorkState), GameState.Work}
        };
        
        gameStateMachine.OnStateChange.SubscribeWithSkip(currentState =>
        {
            var currentStateType = currentState.GetType();
            var gameState = GameState.Unknown;

            if (gameStateByType.ContainsKey(currentStateType))
                gameState = gameStateByType[currentStateType];
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
