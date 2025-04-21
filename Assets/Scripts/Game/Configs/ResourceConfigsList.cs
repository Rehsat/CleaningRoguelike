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
        [SerializeField] private SerializableDictionary<Resource, ResourceConfig> _resourceConfigs;

        public ResourceConfig GetResourceConfig(Resource resourceType)
        {
            return _resourceConfigs[resourceType];
        }
    }

    [Serializable]
    public struct ResourceConfig
    {
        [SerializeField] private Sprite _icon;
        public Sprite Icon => _icon;
    }
}