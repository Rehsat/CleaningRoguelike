using Game.Interactables.Contexts;
using Game.Interactables.Factories;
using UnityEngine;

namespace Game.Interactables.Actions
{
    public abstract class CreateBuilableObjectAction<TObjectType> : IAction where TObjectType : Component
    {
        public void ApplyAction(ContextContainer context)
        {
            BuildableBoxesFactory boxesFactory;
            SpawnTransformContext spawnTransformContext;
            
            if (context.TryGetContext(out boxesFactory) && context.TryGetContext(out spawnTransformContext))
            {
                var objectToBuild = GetObjectToBuild(context);
                if(objectToBuild == null) return;

                var box = boxesFactory.Create(objectToBuild.gameObject);
                box.transform.position = spawnTransformContext.SpawnTransform.position;
            }

        }

        protected abstract TObjectType GetObjectToBuild(ContextContainer context);
    }
}
