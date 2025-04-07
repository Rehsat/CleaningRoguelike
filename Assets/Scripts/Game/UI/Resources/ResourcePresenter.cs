using System.Collections;
using System.Collections.Generic;
using Game.UI.Resources;
using UniRx;
using UnityEngine;

public class ResourcePresenter 
{
    public ResourcePresenter(IReadOnlyReactiveProperty<float> resource, IResourceView resourceView)
    {
        resource.Subscribe(resourceView.UpdateResourceValue);
    }
}