using Game.Interactables.Factories;

namespace Game.Interactables.Actions
{
    public abstract class CreateBuilableObjectAction<TObjectType> : IAction
    {
        public void ApplyAction(ContextContainer context)
        {
            BuildableBoxesFactory boxesFactory;
            if (context.TryGetContext(out boxesFactory))
            {
                var objectToBuild = GetObjectToBuild(context);
                if(objectToBuild == null) return;

                var box = boxesFactory;
            }

        }

        protected abstract TObjectType GetObjectToBuild(ContextContainer context);
    }
}
