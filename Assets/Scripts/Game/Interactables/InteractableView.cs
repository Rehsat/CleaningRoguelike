using System.Collections.Generic;
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

        private ContextContainer _contextContainer;
        private List<IActionContainer> _actionsOnInteract = new List<IActionContainer>();
        private void Start()
        {
            _contextContainer = new ContextContainer();
            _contextContainer.AddContext(new InteractedObjectContext(this));
            _unselectedColor = _outline.OutlineColor;
            _startOutlineWidth = _outline.OutlineWidth;
        }

        public InteractableView AddActionApplier(IActionContainer actionContainer)
        {
            _actionsOnInteract.Add(actionContainer);
            return this;
        }
        public void SetIsSelectedState(bool isSelected)
        {
            _outline.OutlineColor = isSelected ? Color.white : _unselectedColor;
            _outline.OutlineWidth = isSelected ? _startOutlineWidth * 3 : _startOutlineWidth;
        }

        public void Interact(ContextContainer context)
        {
            context.AddContext(_contextContainer);
            _actionsOnInteract.ForEach(action => action.ApplyAction(context));
            OnInteract();
        }

        protected virtual void OnInteract(){}

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
    }

    public interface IInteractableContext{}

    public interface IContextContainer
    {
        public ContextContainer AddContext<TContext>(TContext context) where TContext : IInteractableContext;
    }
}
