using System;
using Game.Interactables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Clothing
{
    public class WashingMachine : InteractableView
    {
        [SerializeField] private WorkMode _workMode;
        [SerializeField] private ParticleSystem _particleOnComplete;
        [SerializeField] private ClothingChangerSettings _clothingChangerSettings;
        private ChangeClothingStateAction _clothingChangeAction;
        protected override void OnConstruct()
        {
            _clothingChangeAction = new ChangeClothingStateAction(_clothingChangerSettings, transform.forward);
            IWorkAction workAction = null;
            
            AddActionApplier(_clothingChangeAction, Interaction.Collide);
            if (_workMode == WorkMode.Automatic)
            {
                workAction = new TimedAction<ChangeClothingStateAction>(_clothingChangeAction, 2);
                AddActionApplier(workAction, Interaction.Collide);
            }
            else
            {
                workAction = new PressingStateSwitchAction<ChangeClothingStateAction>(_clothingChangeAction, 10);
                AddActionApplier(workAction, Interaction.OnLookStateChange);
            }
            
            workAction.OnWorkStateChanged.SubscribeWithSkip((workIsEnabled => // on work end
            {
                if (workIsEnabled)
                {
                    _particleOnComplete.gameObject.SetActive(false);
                }
                else
                {
                    var particlePosition = _clothingChangerSettings.DropPosition.position;
                    _particleOnComplete.transform.position = particlePosition;
                    _particleOnComplete.gameObject.SetActive(true);
                }
            }));
        }

        protected override bool CanBeInteracted(ContextContainer context, Interaction interactionType)
        {
            var canClothingBeChanged = context.TryGetContext<ClothingContext>(out var clothing)
                                       && clothing.Clothing.CurrentClothingStage == _clothingChangerSettings.StageToApply
                                       && _clothingChangeAction.HasClothing == false;
            
            var isAutomaticReady = canClothingBeChanged || 
                                   interactionType == Interaction.OnLookStateChange && _clothingChangeAction.HasClothing;
            if (isAutomaticReady)
            {
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
        [SerializeField] private Mesh _resultMesh;
        [SerializeField] private float _dropSpeed;
        
        public ClothingStage StageToApply => _stageToApply;
        public ClothingStage ResultStage => _resultStage;
        public Transform DropPosition => _dropPosition;
        public Mesh ResultMesh => _resultMesh;
        public float DropSpeed => _dropSpeed;
    }

    public enum ClothingStage
    {
        Dirty,
        Clean,
    }

    public enum WorkMode
    {
        Manual,
        Automatic
    }
}
