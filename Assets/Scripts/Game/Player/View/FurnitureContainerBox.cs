using System.Collections;
using System.Collections.Generic;
using Game.Interactables;
using UnityEngine;

public class FurnitureContainerBox : InteractableView
{
    public GameObject BuildableObjectPrefab;
    protected override void OnConstruct()
    {
        base.OnConstruct();
        AddActionApplier(new PickUpAction());
    }
}
