using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Interactables
{
    public class ContextContainer : IContextContainer
    {
        private Dictionary<Type, IInteractableContext> _contexts;
        public ContextContainer()
        {
            _contexts = new Dictionary<Type, IInteractableContext>();
        }

        public ContextContainer AddContext<TContext>(TContext context) where TContext : IInteractableContext
        {
            _contexts.Add(typeof(TContext), context);
            return this;
        }

        public ContextContainer AddContext(ContextContainer newContextContainer)
        {
            var newContexts = newContextContainer.GetAllContexts();
            newContexts.ForEach(context => AddContext(context));
            return this;
        }

        public List<IInteractableContext> GetAllContexts()
        {
            return _contexts.Values.ToList();
        }

        public bool TryGetContext<TContext>(out TContext context) where TContext : IInteractableContext
        {
            if (_contexts.ContainsKey(typeof(TContext)))
            {
                context = (TContext)_contexts[typeof(TContext)];
                return true;
            }
            else
            {
                context = default;
                return false;
            }
        }
    }
}