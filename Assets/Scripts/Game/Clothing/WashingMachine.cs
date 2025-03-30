using Game.Interactables;
using UnityEngine;

namespace Game.Clothing
{
    public class WashingMachine : InteractableView
    {
        [SerializeField] private Collider _detectorCollider;
        [SerializeField] private Transform _dropPosition;
       // [SerializeField] private 
        [SerializeField] private float _dropSpeed;

        protected override void OnConstruct()
        {
            var clothingChangeAction = new ChangeClothingState();
            AddActionApplier(new TimedAction<ChangeClothingState>(clothingChangeAction, 2));
        }
    }
}
