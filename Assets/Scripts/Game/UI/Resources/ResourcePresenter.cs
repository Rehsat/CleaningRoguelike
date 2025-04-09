using System.Collections;
using System.Collections.Generic;
using Game.UI.Resources;
using UniRx;
using UnityEngine;

public class ResourcePresenter 
{
    public ResourcePresenter(ResourceData resource, ResourceView resourceView)
    {
        resource.CurrentValue.Subscribe(resourceValue=>
        {
            resourceView.UpdateResourceValue(resourceValue, resource.MaxValue.Value);
        });
    }
}