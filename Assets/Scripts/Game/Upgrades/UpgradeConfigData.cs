using System;
using UnityEngine;

namespace Game.Upgrades
{
    [Serializable]
    public struct UpgradeConfigData
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public float Cost { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}