using Gasme.Configs;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(menuName = "GameConfigs/GameGlobalConfig", fileName = "GameGlobalConfig")]
    public class GameGlobalConfig : ScriptableObject
    {
        [field:SerializeField] public PrefabsContainer  PrefabsContainer { get; private set; }
        [field:SerializeField] public ResourceConfigsList  ResourceConfigsList { get; private set; }
    }
}
