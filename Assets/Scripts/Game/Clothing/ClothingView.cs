using Game.Interactables;
using UnityEngine;

namespace Game.Clothing
{
    [RequireComponent(typeof(Rigidbody))]
    public class ClothingView : InteractableView
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ColliderContextDeliver _colliderContextDeliver;
        [SerializeField] private ClothingStage _currentClothingStage;
        public ClothingStage CurrentClothingStage => _currentClothingStage;
        public Rigidbody Rigidbody => _rigidbody;
        protected override void OnConstruct()
        {
            _colliderContextDeliver.Construct();
            MyContextContainer.OnNewContextAdded.SubscribeWithSkip(_colliderContextDeliver.AddContext);
            MyContextContainer.AddContext(new ClothingContext(this));
        }

        public void SetCurrentClothingStage(ClothingStage currentClothingStage)
        {
            _currentClothingStage = currentClothingStage;
        }

        public void OnValidate()
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();
        }
    }
}