using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(menuName = "GameConfigs/ResourceConfigsList", fileName = "ResourceConfigsList")]
    public class ResourceConfigsList : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<PlayerValue, ResourceConfig> _resourceConfigs;

        public ResourceConfig GetResourceConfig(PlayerValue playerValueType)
        {
            return _resourceConfigs[playerValueType];
        }
    }

    [Serializable]
    public struct ResourceConfig
    {
        [SerializeField] private bool _hasMaximum;
        [SerializeField] private Sprite _icon;
        [field: SerializeField] public ResourceObserve ObserveType;
        public Sprite Icon => _icon;
        public bool HasMaximum => _hasMaximum;
    }

    public enum ResourceObserve
    {
        None = 0,
        OnlyCurrentValue = 1,
        OnlyMaxValue = 2
    }
}