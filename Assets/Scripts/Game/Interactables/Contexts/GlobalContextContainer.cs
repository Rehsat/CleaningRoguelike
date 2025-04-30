using System.Collections.Generic;
using Zenject;

namespace Game.Interactables.Contexts
{
    public class GlobalContextContainer : IContextContainer
    {
        private ContextContainer _contextContainer;
        public ContextContainer ContextContainer => _contextContainer;
        [Inject]


        public GlobalContextContainer(List<IContext> contexts)
        {
            _contextContainer = new ContextContainer();
            contexts.ForEach(context => _contextContainer.AddContext(context));
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

    public class GameValuesContext : IContext
    {
        public GameValuesContainer GameResources { get; private set; }
        [Inject]
        public GameValuesContext(GameValuesContainer gameResources)
        {
            GameResources = gameResources;
        }
    }
}
