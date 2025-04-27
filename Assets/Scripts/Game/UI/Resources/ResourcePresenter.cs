using System.Collections;
using System.Collections.Generic;
using Game.UI.Resources;
using UniRx;
using UnityEngine;

public class ResourcePresenter 
{
    public ResourcePresenter(PlayerGameValueData playerGameValue, IResourceView resourceView)
    {
        resourceView.SetIcon(playerGameValue.Config.Icon);
        playerGameValue.CurrentValue.Subscribe(resourceValue=>
        {
            resourceView.UpdateResourceValue(resourceValue, playerGameValue.MaxValue.Value);
        });
    }
}