using System;
using Game.GameStateMachine;
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
        [SerializeField] private Transform _dropPosition;
        private ChangeClothingStateAction _clothingChangeAction;

        public Transform DropPosition => _dropPosition;

        // TODO отрефакторить, чтоб был констракт в фабрике
        protected override void OnConstruct()
        {
            Debug.LogError(1);
            _clothingChangeAction = new ChangeClothingStateAction(_clothingChangerConfig, _dropPosition, transform.forward);
            IWorkAction workAction = null;
            
            AddActionApplier(_clothingChangeAction, Interaction.Collide);
            AddActionApplier(new ChangeSellablePrice(20), Interaction.Collide);
            if (_workMode == WorkMode.Automatic)
            {
                workAction = new TimedAction(_clothingChangeAction, 25);
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

        private void OnWorkEnabledStateChange(WorkState workState)
        {
            SetWorkProgressViewByState(workState);
            
            if (workState == WorkState.Completed)
            {
                var particlePosition = _dropPosition.position;
                _particleOnComplete.transform.position = particlePosition;
                _particleOnComplete.gameObject.SetActive(true);
                _progressBarView.gameObject.SetActive(false);
            }
            else
            {
                _particleOnComplete.gameObject.SetActive(false);
            }
        }

        private void SetWorkProgressViewByState(WorkState workState)
        {
            if (workState == WorkState.Started)
                _progressBarView.transform.DoShowAnimation();
            else
                _progressBarView.transform.DoHideAnimation();
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
        [SerializeField] private Mesh _resultMesh;
        [SerializeField] private float _dropSpeed;
        
        public ClothingStage StageToApply => _stageToApply;
        public ClothingStage ResultStage => _resultStage;
        public Mesh ResultMesh => _resultMesh;
        public float DropSpeed => _dropSpeed;
        public ClothingChangerConfig(
            ClothingStage stageToApply,
            ClothingStage resultStage,
            Mesh resultMesh,
            float dropSpeed)
        {
            _stageToApply = stageToApply;
            _resultStage = resultStage;
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
