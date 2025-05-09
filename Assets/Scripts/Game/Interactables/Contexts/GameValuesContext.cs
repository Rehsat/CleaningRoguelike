using UnityEngine;
using Zenject;

namespace Game.Interactables.Contexts
{
    public class GameValuesContext : IContext
    {
        public GameValuesContainer GameResources { get; private set; }
        [Inject]
        public GameValuesContext(GameValuesContainer gameResources)
        {
            GameResources = gameResources;
        }
    }

    public class SpawnTransformContext : IContext
    {
        public Transform SpawnTransform { get; private set; }

        public SpawnTransformContext(Transform spawnTransform)
        {
            SpawnTransform = spawnTransform;
        }
    }
}