using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

namespace Gasme.Configs
{
    [CreateAssetMenu(menuName = "GameConfigs/PrefabsContainer", fileName = "PrefabsContainer")]
    public class PrefabsContainer : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<Prefab, GameObject> _prefabs;
        public GameObject GetPrefab(Prefab prefabType)
        {
            return _prefabs[prefabType];
        }

        public TComponent GetPrefabsComponent<TComponent>(Prefab prefabType) where TComponent : Component
        {
            if (GetPrefab(prefabType).TryGetComponent<TComponent>(out var component))
            {
                return component;
            }
            else
            {
                Debug.LogError($"There is no component{typeof(TComponent)} on {prefabType}");
                return null;
            }
            
        }
    }

    public enum Prefab
    {
        SellParticle = 0,
        WashingMachine = 1,
        
    }
}
