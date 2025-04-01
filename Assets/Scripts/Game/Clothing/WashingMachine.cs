using System;
using Game.Interactables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Clothing
{
    public class WashingMachine : InteractableView
    {
        [SerializeField] private ClothingChangerSettings _clothingChangerSettings;
        protected override void OnConstruct()
        {
            var clothingChangeAction = new ChangeClothingStateAction(_clothingChangerSettings);
            AddActionApplier(new TimedAction<ChangeClothingStateAction>(clothingChangeAction, 2), Interaction.Collide);
        }

        protected override bool CanBeInteracted(ContextContainer context, Interaction interactionType)
        {
            if (context.TryGetContext<ClothingContext>(out var clothing) 
                && clothing.Clothing.CurrentClothingStage == _clothingChangerSettings.StageToApply)
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
        [SerializeField] private ClothingStage _resultStage;
        [SerializeField] private Transform _dropPosition;
        [SerializeField] private float _dropSpeed;
        
        public ClothingStage StageToApply => _stageToApply;
        public ClothingStage ResultStage => _resultStage;
        public Transform DropPosition => _dropPosition;
        public float DropSpeed => _dropSpeed;
    }

    public enum ClothingStage
    {
        Dirty,
        Clean,
    }
}
