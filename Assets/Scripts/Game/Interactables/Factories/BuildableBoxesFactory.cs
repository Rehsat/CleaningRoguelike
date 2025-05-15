using Game.Interactables.Stackable;
using Game.Player.View;
using Gasme.Configs;
using UnityEngine;
using Zenject;

namespace Game.Interactables.Factories
{
    public class BuildableBoxesFactory : IFactory<GameObject, FurnitureContainerBox>, IContext
    {
        private readonly FurnitureContainerBox _boxPrefab;
        private readonly InteractablesMergeService _mergeService;

        public BuildableBoxesFactory(PrefabsContainer prefabsContainer, InteractablesMergeService mergeService)
        {
            _boxPrefab = prefabsContainer.GetPrefabsComponent<FurnitureContainerBox>(Prefab.BuildableBox);
            _mergeService = mergeService;
        }

        public FurnitureContainerBox Create(GameObject objectToBuild)
        {
            var box = Object.Instantiate(_boxPrefab);
            box.Construct();
            box
                .AddActionApplier(new PickUpAction());
            
            box.SetBuildableObject(objectToBuild);
            return box;
        }
    }
}