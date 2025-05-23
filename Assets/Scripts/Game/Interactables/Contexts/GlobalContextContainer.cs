using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Interactables.Contexts
{
    public class GlobalContextContainer : IContextContainer
    {
        private ContextContainer _contextContainer;
        public ContextContainer ContextContainer => _contextContainer;
        [Inject]
        public GlobalContextContainer(List<IContext> contexts, List<IGlobalContextListener> listeners)
        {
            _contextContainer = new ContextContainer();
            contexts.ForEach(context => _contextContainer.AddContext(context));
            listeners.ForEach(listener => listener.SetContext(this));
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
    }
}
