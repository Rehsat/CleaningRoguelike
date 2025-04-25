using Zenject;
using UniRx;
namespace Game.Upgrades
{
    public class UpgradeController
    {
        private readonly UpgradesSelector _upgradesSelector;

        [Inject]
        public UpgradeController(UpgradesSelector upgradesSelector, SceneObjectsContainer sceneObjectsContainer)
        {
            _upgradesSelector = upgradesSelector;
            var upgradeView = sceneObjectsContainer.GetObjectsComponent<IUpgradeView>(SceneObject.UpgradeView);
            Init(upgradesSelector, upgradeView);
        }
        private void Init(UpgradesSelector upgradesSelector, IUpgradeView upgradeView)
        {
            upgradesSelector.CurrentUpgrades.Subscribe(upgradeView.SetUpgrades);
        }

        public void SelectNewUpgrades(int upgradesCount)
        {
            _upgradesSelector.SelectNewUpgrades(upgradesCount);
        }
    }
}
