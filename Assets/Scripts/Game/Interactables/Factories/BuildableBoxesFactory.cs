using Game.Player.View;
using UnityEngine;
using Zenject;

namespace Game.Interactables.Factories
{
    public class BuildableBoxesFactory : IFactory<GameObject, FurnitureContainerBox>, IContext
    {
        private readonly FurnitureContainerBox _boxPrefab;

        public BuildableBoxesFactory(FurnitureContainerBox boxPrefab)
        {
            _boxPrefab = boxPrefab;
        }

        public FurnitureContainerBox Create(GameObject objectToBuild)
        {
            var box = Object.Instantiate(_boxPrefab);
            box.SetBuildableObject(objectToBuild);
            return box;
        }
    }
}