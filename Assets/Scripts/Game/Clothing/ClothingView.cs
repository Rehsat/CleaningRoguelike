using Game.Interactables;
using UnityEngine;

namespace Game.Clothing
{
    public class ClothingView : InteractableView
    {
        [SerializeField] private ColliderContextDeliver _colliderContextDeliver;
        [SerializeField] private ClothingStage _currentClothingStage;
        public ClothingStage CurrentClothingStage => _currentClothingStage;
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
    }
}