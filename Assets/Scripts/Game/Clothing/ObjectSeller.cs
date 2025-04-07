using Game.Interactables;
using Game.Interactables.Contexts;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game.Clothing
{
    public class ObjectSeller
    {
        private readonly Collider _sellCollider;

        public ObjectSeller(Collider sellCollider, PlayerResources resources)
        {
            _sellCollider = sellCollider;
            _sellCollider.OnTriggerEnterAsObservable().Subscribe(potentialSellable =>
            {
                if(potentialSellable.TryGetComponent<IContextContainer>(out var contextContainer) == false) return;
                if (contextContainer.TryGetContext<SellableContext>(out var sellable))
                {
                    potentialSellable.gameObject.SetActive(false);
                    resources.ChangeResourceBy(Resource.QuotaMoney, sellable.Cost.Value);
                }

            });
        }
    }
}
