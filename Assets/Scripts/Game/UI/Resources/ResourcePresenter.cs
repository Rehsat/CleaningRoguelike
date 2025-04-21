using System.Collections;
using System.Collections.Generic;
using Game.UI.Resources;
using UniRx;
using UnityEngine;

public class ResourcePresenter 
{
    public ResourcePresenter(ResourceData resource, IResourceView resourceView)
    {
        resourceView.SetIcon(resource.Config.Icon);
        resource.CurrentValue.Subscribe(resourceValue=>
        {
            resourceView.UpdateResourceValue(resourceValue, resource.MaxValue.Value);
        });
    }
}