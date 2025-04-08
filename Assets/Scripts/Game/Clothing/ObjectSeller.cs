using Game.Interactables;
using Game.Interactables.Contexts;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game.Clothing
{
    public class ObjectSeller
    {
        private readonly PlayerResources _resources;
        private readonly ParticleSystem _sellParticle;

        public ObjectSeller(PlayerResources resources, PrefabsContainer prefabsContainer)
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
                    _resources.ChangeResourceBy(Resource.QuotaMoney, sellable.Cost.Value);
                    _sellParticle.transform.position = potentialSellable.transform.position;
                    SpawnParticle(sellable.Cost.Value);
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
