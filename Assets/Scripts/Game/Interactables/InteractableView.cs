using System;
using System.Collections.Generic;
using System.Linq;
using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using Game.Interactables.Stackable;
using Game.Player.Data;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Interactables
{
    [RequireComponent(typeof(Outline))]
    public class InteractableView : MonoBehaviour, IContextContainer, IStackable, IActionsContainer
    {
        [SerializeField] private Outline _outline;
        [SerializeField] private bool aimHelpEnabled;
        private Color _unselectedColor;
        private float _startOutlineWidth;
        private bool _wasConstructed;

        private ReactiveProperty<bool> _isEnabled;
        private ContextContainer _contextContainer;
        private Dictionary<Interaction, List<IAction>> _actions;

        protected ContextContainer MyContextContainer => _contextContainer;
        public bool AimHelpEnabled => aimHelpEnabled;

        private void Start()
        {
            Construct();
        }

        public void Construct()
        {
            if(_wasConstructed) return;
            _wasConstructed = true;
            
            _isEnabled = new ReactiveProperty<bool>(true);
            _isEnabled.Subscribe((isEnabled =>
            {
                _outline.enabled = isEnabled;
            }));
            
            _actions = new Dictionary<Interaction, List<IAction>>();
            foreach (Interaction interaction in Enum.GetValues(typeof(Interaction)))
                _actions.Add(interaction, new List<IAction>());
            
            _unselectedColor = _outline.OutlineColor;
            _startOutlineWidth = _outline.OutlineWidth;
            
            _contextContainer = new ContextContainer();
            _contextContainer.AddContext(new InteractedObjectContext(this));
            
            OnConstruct();
        }

        public InteractableView AddActionApplier(IAction action, Interaction interactionType = Interaction.InteractButton)
        {
            _actions[interactionType].Add(action);
            return this;
        }
        public void SetIsSelectedState(bool isSelected)
        {
            var selectedColor = 
                _actions[Interaction.InteractButton].Count > 0 ? Color.white : Color.green;
            _outline.OutlineColor = isSelected ? selectedColor : _unselectedColor;
            _outline.OutlineWidth = isSelected ? _startOutlineWidth * 3 : _startOutlineWidth;
        }

        public void SetEnabled(bool isEnabled)
        {
            _isEnabled.Value = isEnabled;
        }

        public void Interact(ContextContainer context, Interaction interactionType)
        {
            if(_isEnabled.Value == false || CanBeInteracted(context,interactionType) == false) return;
            
            var resultContext = new ContextContainer()
                .AddContext(_contextContainer)
                .AddContext(context);
            
            _actions[interactionType].ForEach(action => action.ApplyAction(resultContext));
            OnInteract(resultContext, interactionType);
        }
        public ContextContainer AddContext<TContext>(TContext context) where TContext : IContext
        {
            _contextContainer.AddContext(context);
            return _contextContainer;
        }

        public bool TryGetContext<TContext>(out TContext context) where TContext : IContext
        {
            return _contextContainer.TryGetContext(out context);
        }


        public bool TryGetAction(Type actionType, out IAction action)
        {
            action = default;
            foreach (var item in _actions.Values.SelectMany(listOfAction => listOfAction))
            {
                if (item.GetType() == actionType)
                {
                    action = item;
                    return true;
                }
            }
            return false;
        }
        /*public bool TryGetAction<TActionType>(out TActionType action) where TActionType : IAction
        {
            action = default;
            foreach (var item in _actions.Values.SelectMany(listOfAction => listOfAction))
            {
                if (item is TActionType typedAction)
                {
                    action = typedAction;
                    return true;
                }
            }
            return false;
        }*/
        public void Stack(IStackable objectToStackWith)
        {
            if (objectToStackWith is IActionsContainer actionsContainer)
            {
                var allActions = GetAllActions();
                allActions.ForEach(action =>
                {
                    if (action is IStackable stackable)
                    {
                        if(actionsContainer.TryGetAction(action.GetType(), out var secondAction))
                        {
                            stackable.Stack((IStackable)secondAction);
                        }
                    }
                });
            }
        }
        private List<IAction> GetAllActions()
        {
            List<IAction> allActions = new List<IAction>();
        
            foreach (var actionList in _actions.Values)
            {
                if (actionList != null)
                {
                    allActions.AddRange(actionList);
                }
            }
        
            return allActions;
        }

        protected virtual void OnConstruct(){}
        protected virtual bool CanBeInteracted(ContextContainer context, Interaction interactionType){return true;}
        protected virtual void OnInteract(ContextContainer context, Interaction interactionType){}

        public void OnValidate()
        {
            if (_outline == null)
                _outline = GetComponent<Outline>();
        }
    }

    public enum Interaction
    {
        InteractButton,
        Collide,
        OnLookStateChange
    }
}
