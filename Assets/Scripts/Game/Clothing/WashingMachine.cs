using System;
using Game.Interactables;
using Game.UI.Interactables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Clothing
{
    public class WashingMachine : InteractableView
    {
        [SerializeField] private WorkMode _workMode;
        [SerializeField] private ParticleSystem _particleOnComplete;
        [SerializeField] private ProgressBarView _progressBarView;
        [SerializeField] private ClothingChangerConfig _clothingChangerConfig;
        private ChangeClothingStateAction _clothingChangeAction;
        
        // TODO отрефакторить, чтоб был констракт в фабрике
        protected override void OnConstruct()
        {
            _clothingChangeAction = new ChangeClothingStateAction(_clothingChangerConfig, transform.forward);
            IWorkAction workAction = null;
            
            AddActionApplier(_clothingChangeAction, Interaction.Collide);
            AddActionApplier(new ChangeSellablePrice(20), Interaction.Collide);
            if (_workMode == WorkMode.Automatic)
            {
                workAction = new TimedAction<ChangeClothingStateAction>(_clothingChangeAction, 25);
                AddActionApplier(workAction, Interaction.Collide);
            }
            else
            {
                workAction = new PressingStateSwitchAction<ChangeClothingStateAction>(_clothingChangeAction, 10);
                AddActionApplier(workAction, Interaction.OnLookStateChange);
            }
            
            workAction.OnWorkStateChanged.SubscribeWithSkip(OnWorkEnabledStateChange);
            _progressBarView.gameObject.SetActive(false);
            var progressBarPresenter = new ProgressPresenter(workAction, _progressBarView);
        }

        public void SetConfig(ClothingChangerConfig config)
        {
            _clothingChangerConfig = config;
            
        }

        private void OnWorkEnabledStateChange(bool isWorkEnabled)
        {
            _progressBarView.gameObject.SetActive(isWorkEnabled);
            if (isWorkEnabled)
            {
                _particleOnComplete.gameObject.SetActive(false);
            }
            else
            {
                var particlePosition = _clothingChangerConfig.DropPosition.position;
                _particleOnComplete.transform.position = particlePosition;
                _particleOnComplete.gameObject.SetActive(true);
            }
        }

        protected override bool CanBeInteracted(ContextContainer context, Interaction interactionType)
        {
            var canClothingBeChanged = context.TryGetContext<ClothingContext>(out var clothing)
                                       && clothing.Clothing.CurrentClothingStage == _clothingChangerConfig.StageToApply
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
    public struct ClothingChangerConfig
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
        public ClothingChangerConfig(
            ClothingStage stageToApply,
            ClothingStage resultStage,
            Transform dropPosition,
            Mesh resultMesh,
            float dropSpeed)
        {
            _stageToApply = stageToApply;
            _resultStage = resultStage;
            _dropPosition = dropPosition;
            _resultMesh = resultMesh;
            _dropSpeed = dropSpeed;
        }
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
