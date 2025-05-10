using Game.Clothing;
using Game.Interactables.Factories;

namespace Game.Interactables.Actions
{
    public class SpawnWashingMachineAction : CreateBuilableObjectAction<WashingMachine>
    {
        private readonly ClothingChangerConfig _clothingChangerConfig;

        public SpawnWashingMachineAction(ClothingChangerConfig clothingChangerConfig)
        {
            _clothingChangerConfig = clothingChangerConfig;
        }
        protected override WashingMachine GetObjectToBuild(ContextContainer context)
        {
            WashingMachineFactory washingMachineFactory;
            if (context.TryGetContext(out washingMachineFactory))
            {
                var washingMachine = washingMachineFactory.Create(_clothingChangerConfig);
                return washingMachine;
            }

            return null;
        }
    }
}