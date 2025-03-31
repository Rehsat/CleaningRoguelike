using System;
using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using Game.Player.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Interactables
{
    [RequireComponent(typeof(Outline))]
    public class InteractableView : MonoBehaviour, IContextContainer
    {
        [SerializeField] private Outline _outline;

        private Color _unselectedColor;
        private float _startOutlineWidth;
        private bool _wasConstructed;

        private ContextContainer _contextContainer;
        private Dictionary<Interaction, List<IAction>> _actions;

        protected ContextContainer MyContextContainer => _contextContainer;
        private void Start()
        {
            Construct();
        }

        public void Construct()
        {
            if(_wasConstructed) return;
            _wasConstructed = true;

            _actions = new Dictionary<Interaction, List<IAction>>();
            foreach (Interaction interaction in Enum.GetValues(typeof(Interaction)))
                _actions.Add(interaction, new List<IAction>());
            
            _unselectedColor = _outline.OutlineColor;
            _startOutlineWidth = _outline.OutlineWidth;
            
            _contextContainer = new ContextContainer();
            OnConstruct();
            
            _contextContainer.AddContext(new InteractedObjectContext(this));
        }

        protected virtual void OnConstruct(){}

        public InteractableView AddActionApplier(IAction action, Interaction interactionType = Interaction.InteractButton)
        {
            _actions[interactionType].Add(action);
            return this;
        }
        public void SetIsSelectedState(bool isSelected)
        {
            _outline.OutlineColor = isSelected ? Color.white : _unselectedColor;
            _outline.OutlineWidth = isSelected ? _startOutlineWidth * 3 : _startOutlineWidth;
        }

        public void Interact(ContextContainer context, Interaction interactionType = Interaction.InteractButton)
        {
            if(CanBeInteracted(context,interactionType) == false) return;
            context.AddContext(_contextContainer);
            _actions[interactionType].ForEach(action => action.ApplyAction(context));
            OnInteract(context);
        }

        protected virtual bool CanBeInteracted(ContextContainer context, Interaction interactionType){return true;}
        protected virtual void OnInteract(ContextContainer context){}

        public void OnValidate()
        {
            if (_outline == null)
                _outline = GetComponent<Outline>();
        }

        public ContextContainer AddContext<TContext>(TContext context) where TContext : IInteractableContext
        {
            _contextContainer.AddContext(context);
            return _contextContainer;
        }

        public bool TryGetContext<TContext>(out TContext context) where TContext : IInteractableContext
        {
            return _contextContainer.TryGetContext(out context);
        }
    }

    public enum Interaction
    {
        InteractButton,
        Collide
    }
}
