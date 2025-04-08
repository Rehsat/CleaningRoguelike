using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "GameData/PrefabsContainer", fileName = "PrefabsContainer")]
    public class PrefabsContainer : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<Prefab, GameObject> _prefabs;
        public GameObject GetPrefab(Prefab prefabType)
        {
            return _prefabs[prefabType];
        }
    }

    public enum Prefab
    {
        SellParticle = 0,
        
    }
}
