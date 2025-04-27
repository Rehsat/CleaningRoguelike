using System;
using System.Collections;
using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using Game.Configs;
using Game.Interactables;
using UniRx;
using UnityEngine;
using Zenject;

public class GameValuesContainer
{
    private Dictionary<PlayerValue, PlayerGameValueData> _playerResources;

    [Inject]
    public GameValuesContainer(ResourceConfigsList resourceConfigsList)
    {
        _playerResources = new Dictionary<PlayerValue, PlayerGameValueData>();
        foreach (PlayerValue interaction in Enum.GetValues(typeof(PlayerValue)))
            _playerResources.Add(
                interaction, 
                new PlayerGameValueData(5, resourceConfigsList.GetResourceConfig(interaction)));
    }

    public PlayerGameValueData GetPlayerValue(PlayerValue playerValueType)
    {
        return _playerResources[playerValueType];
    }
}

public class PlayerGameValueData
{
    private readonly ResourceConfig _config;
    
    private ReactiveProperty<float> _currentValue;
    private ReactiveProperty<float> _maxValue;
    private ReactiveEvent<float> _onValueChanged;

    private float _lastValue;

    public ResourceConfig Config => _config;
    public IReadOnlyReactiveProperty<float> CurrentValue => _currentValue;
    public IReadOnlyReactiveProperty<float> MaxValue => _maxValue;

    public IReadOnlyReactiveEvent<float> OnValueChanged => _onValueChanged;

    public bool IsCurrentValueMaximum => _currentValue.Value >= _maxValue.Value;

    public PlayerGameValueData(float maxValue, ResourceConfig config)
    {
        _config = config;
        _currentValue= new ReactiveProperty<float>(0);
        _maxValue = new ReactiveProperty<float>(maxValue);
        _lastValue = 0;
        _onValueChanged = new ReactiveEvent<float>();
        _currentValue.Subscribe(newValue =>
        {
            var difference = newValue - _lastValue;
            _lastValue = newValue;
            _onValueChanged.Notify(difference);
        });
    }

    public void ChangeValueBy(float value)
    {
        SetCurrentValue(_currentValue.Value + value);
    }

    public void SetCurrentValue(float value)
    {
        if (_config.HasMaximum && value > _maxValue.Value)
        {
            _currentValue.Value = _maxValue.Value;
            return;
        }
        _currentValue.Value = value;
    }

    public void ChangeMaxValueBy(float value)
    {
        _maxValue.Value += value;
    }

    public void SetMaxValue(float value)
    {
        _maxValue.Value = value;
    }
}
public enum PlayerValue
{
    QuotaMoney = 0,
    UpgradeMoney = 1,
    ActiveClothing = 2,
    None = 3,
    Income= 4,
    PlayerSpeed = 5,
    PlayerThrowPower = 6,
}