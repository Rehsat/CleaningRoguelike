using Game.Interactables;
using UnityEngine;

namespace Game.Clothing
{
    [RequireComponent(typeof(Rigidbody), typeof(MeshUpdater))]
    public class ClothingView : InteractableView
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshUpdater _meshUpdater;
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

        public void SetCurrentClothingStage(ClothingStage currentClothingStage, Mesh mesh)
        {
            _currentClothingStage = currentClothingStage;
            _meshUpdater.UpdateMesh(mesh);
        }

        public void OnValidate()
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();
            if (_meshUpdater == null)
                _meshUpdater = GetComponent<MeshUpdater>();
        }
    }
}