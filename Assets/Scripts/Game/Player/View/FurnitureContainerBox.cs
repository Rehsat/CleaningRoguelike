using Game.Interactables;
using UnityEngine;

namespace Game.Player.View
{
    public class FurnitureContainerBox : InteractableView
    {
        public GameObject BuildableObjectPrefab { get; private set; }

        protected override void OnConstruct()
        {
            base.OnConstruct();
            AddActionApplier(new PickUpAction());
        }

        public void SetBuildableObject(GameObject buildable)
        {
            BuildableObjectPrefab = buildable;
        }
    }
}
