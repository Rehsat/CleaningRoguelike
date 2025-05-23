using Game.Configs;
using Game.Interactables;
using Game.Interactables.Contexts;
using Gasme.Configs;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game.Clothing
{
    public class ObjectSeller
    {
        private readonly GameValuesContainer _resources;
        private readonly ParticleSystem _sellParticle;

        public ObjectSeller(GameValuesContainer resources, PrefabsContainer prefabsContainer)
        {
            _resources = resources;
            var particlePrefab = prefabsContainer.GetPrefab(Prefab.SellParticle);
            _sellParticle = Object.Instantiate(particlePrefab).GetComponent<ParticleSystem>();
        }

        public void SetSellCollider(Collider sellCollider)
        {
            sellCollider.OnTriggerEnterAsObservable().Subscribe(potentialSellable =>
            {
                if(potentialSellable.TryGetComponent<IContextContainer>(out var contextContainer) == false) return;
                if (contextContainer.TryGetContext<SellableContext>(out var sellable))
                {
                    potentialSellable.gameObject.SetActive(false);
                    _resources.GetPlayerValue(PlayerValue.QuotaMoney).ChangeValueBy(sellable.Cost.Value);
                    _sellParticle.transform.position = potentialSellable.transform.position;
                    SpawnParticle(sellable.Cost.Value);
                }

                if (contextContainer.TryGetContext<ReturnableContext>(out var returnableContext))
                {
                    returnableContext.Return();
                }

            });
        }

        private void SpawnParticle(float cost)
        {
            var emission = _sellParticle.emission;
            ParticleSystem.Burst burst = emission.GetBurst(0);
            burst.count = cost;
            emission.SetBurst(0, burst);
            _sellParticle.Play();
        }
    }
}
