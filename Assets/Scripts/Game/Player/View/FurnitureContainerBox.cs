using Game.Interactables;
using UnityEngine;

namespace Game.Player.View
{
    public class FurnitureContainerBox : InteractableView
    {
        public GameObject BuildableObjectPrefab { get; private set; }

        public void SetBuildableObject(GameObject buildable)
        {
            BuildableObjectPrefab = buildable;
        }
    }
}
