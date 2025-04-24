using Game.Player.Data;

namespace Game.Interactables.Contexts
{
    public class ObjectHolderContext : IContext
    {
        public ObjectHolder ObjectHolder { get; }

        public ObjectHolderContext(ObjectHolder objectHolder)
        {
            ObjectHolder = objectHolder;
        }
    }
}