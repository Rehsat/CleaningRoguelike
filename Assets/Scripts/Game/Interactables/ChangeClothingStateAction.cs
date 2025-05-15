using Game.Clothing;
using UnityEngine;

namespace Game.Interactables
{
    public class ChangeClothingStateAction : IAction
    {
        private readonly ClothingChangerConfig _clothingChangerConfig;
        private readonly Transform _dropPosition;
        private readonly Vector3 _dropDirection;
        private ClothingContext _currentClothingContext;

        public bool HasClothing => _currentClothingContext != null;

        public ChangeClothingStateAction(ClothingChangerConfig clothingChangerConfig, Transform dropPosition, Vector3 dropDirection)
        {
            _clothingChangerConfig = clothingChangerConfig;
            _dropPosition = dropPosition;
            _dropDirection = dropDirection;
        }
        public void ApplyAction(ContextContainer context)
        {
            if (_currentClothingContext != null)
            {
                var clothing = _currentClothingContext.Clothing;
                
                clothing.SetCurrentClothingStage(_clothingChangerConfig.ResultStage, _clothingChangerConfig.ResultMesh);
                
                var clothingGameObject = clothing.gameObject;
                clothingGameObject.transform.position = _dropPosition.position;
                clothingGameObject.SetActive(true);
                
                var dropForce = _dropDirection * _clothingChangerConfig.DropSpeed * 0.3f +
                                Vector3.up * _clothingChangerConfig.DropSpeed;
                clothing.Rigidbody.velocity = Vector3.zero;
                clothing.Rigidbody.AddForce(dropForce, ForceMode.Impulse);

                _currentClothingContext = null;
                return;
            }
            
            if (context.TryGetContext(out _currentClothingContext))
            {
                var clothing = _currentClothingContext.Clothing;
                clothing.gameObject.SetActive(false);
                return;
            }
        }
    }
}