using System;
using System.Collections;
using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using Game.Interactables;
using UniRx;
using UnityEngine;

public class PlayerResources
{
    private Dictionary<Resource, ResourceData> _playerResources;

    public static PlayerResources Instance { get; private set; } // Временно синглтон, пока не придумаю как сделать лучше

    public PlayerResources()
    {
        if(Instance != null) return;
        Instance = this;
        
        _playerResources = new Dictionary<Resource, ResourceData>();
        foreach (Resource interaction in Enum.GetValues(typeof(Resource)))
            _playerResources.Add(interaction, new ResourceData(5));
    }

    public ResourceData GetResource(Resource resourceType)
    {
        return _playerResources[resourceType];
    }
}

public class ResourceData
{ 
    private ReactiveProperty<float> _currentValue;
    private ReactiveProperty<float> _maxValue;

    public IReadOnlyReactiveProperty<float> CurrentValue => _currentValue;
    public IReadOnlyReactiveProperty<float> MaxValue => _maxValue;
    public bool IsCurrentValueMaximum => _currentValue.Value >= _maxValue.Value;

    public ResourceData(float maxValue)
    {
        _currentValue= new ReactiveProperty<float>();
        _maxValue = new ReactiveProperty<float>(maxValue);
    }

    public void ChangeValueBy(float value)
    {
        if (_currentValue.Value + value > _maxValue.Value)
        {
            _currentValue.Value = _maxValue.Value;
            return;
        }
        _currentValue.Value += value;
    }

    public void ChangeMaxValueBy(float value)
    {
        _maxValue.Value += value;
    }
}
public enum Resource
{
    QuotaMoney,
    UpgradeMoney,
    ActiveClothing
}