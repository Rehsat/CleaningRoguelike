using System;
using Game.Interactables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Clothing
{
    public class WashingMachine : InteractableView
    {
        [SerializeField] private Collider _detectorCollider;
        [SerializeField] private ClothingChangerSettings _clothingChangerSettings;
        protected override void OnConstruct()
        {
            var clothingChangeAction = new ChangeClothingState(_clothingChangerSettings);
            AddActionApplier(new TimedAction<ChangeClothingState>(clothingChangeAction, 2));
        }

        protected override bool CanBeInteracted(ContextContainer context, Interaction interactionType)
        {
            if (context.TryGetContext<ClothingContext>(out var clothing))
            {
                clothing.Clothing.gameObject.SetActive(false); // Такое себе решение, но пока прототип - пусть будет
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public class ClothingChangerSettings
    {
        [SerializeField] private ClothingStage _stageToApply;
        [SerializeField] private Mesh _modelOnChange;
        [SerializeField] private Transform _dropPosition;
        [SerializeField] private float _dropSpeed;
        
        public ClothingStage StageToApply => _stageToApply;
        public Mesh ModelOnChange => _modelOnChange;
        public Transform DropPosition => _dropPosition;
        public float DropSpeed => _dropSpeed;
    }

    public enum ClothingStage
    {
        Dirty,
        Clean,
    }
}
