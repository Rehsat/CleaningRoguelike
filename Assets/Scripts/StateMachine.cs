using System;
using System.Collections;
using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using UniRx;
using UnityEngine;

public class StateMachine<T> where T : IState
{
    private ReactiveProperty<IState> _currentState;
    private ReactiveEvent<IState> _onStateChange;
    private CompositeDisposable _compositeDisposable;
    
    protected Dictionary<Type, T> States;

    public ReactiveEvent<IState> OnStateChange => _onStateChange;

    public StateMachine(List<T> states)
    {
        States = new Dictionary<Type, T>();
        _compositeDisposable = new CompositeDisposable();
        _onStateChange = new ReactiveEvent<IState>();
            
        foreach (var state in states)
        {
            States.Add(state.GetType(), state);
        }

        _currentState = new ReactiveProperty<IState>(states[0]);
        _currentState.Subscribe(currentState => _onStateChange.Notify(currentState));
        
        Observable.EveryUpdate().Subscribe(f => _currentState.Value.OnUpdate())
            .AddTo(_compositeDisposable);
    }

    public void EnterState<TState>() where TState : T
    {
        var stateType = typeof(TState);
        if (States.ContainsKey(stateType))
        {
            _currentState.Value.Exit();
            _currentState.Value = States[stateType];
            _currentState.Value.Enter();
        }
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
}
public interface IState
{
    public void Enter();
    public void Exit();
    public void OnUpdate();
        
}
