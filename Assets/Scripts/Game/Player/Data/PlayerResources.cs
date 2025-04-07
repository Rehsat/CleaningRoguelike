using System;
using System.Collections;
using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using Game.Interactables;
using UniRx;
using UnityEngine;

public class PlayerResources
{
    private Dictionary<Resource, ReactiveProperty<float>> _playerResources;

    public static PlayerResources Instance { get; private set; } // Временно синглтон, пока не придумаю как сделать лучше

    public PlayerResources()
    {
        if(Instance != null) return;
        Instance = this;
        
        _playerResources = new Dictionary<Resource, ReactiveProperty<float>>();
        foreach (Resource interaction in Enum.GetValues(typeof(Resource)))
            _playerResources.Add(interaction, new ReactiveProperty<float>());
    }

    public void ChangeResourceBy(Resource resourceType, float value)
    {
        _playerResources[resourceType].Value += value;
    }

    public IReadOnlyReactiveProperty<float> GetResource(Resource resourceType)
    {
        return _playerResources[resourceType];
    }
}

public enum Resource
{
    QuotaMoney,
    UpgradeMoney
}