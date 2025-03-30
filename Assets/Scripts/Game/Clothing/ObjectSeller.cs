using System.Collections;
using System.Collections.Generic;
using Game.Interactables;
using Game.Interactables.Contexts;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ObjectSeller
{
    private readonly Collider _sellCollider;

    public ObjectSeller(Collider sellCollider)
    {
        _sellCollider = sellCollider;
        _sellCollider.OnTriggerEnterAsObservable().Subscribe(potentialSellable =>
        {
            if(potentialSellable.TryGetComponent<IContextContainer>(out var contextContainer) == false) return;
            if (contextContainer.TryGetContext<SellableContext>(out var sellable))
            {
                potentialSellable.gameObject.SetActive(false);
            }

        });
    }
}
