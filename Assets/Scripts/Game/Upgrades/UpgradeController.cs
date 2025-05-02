using Game.Interactables;
using Game.Interactables.Contexts;
using Zenject;
using UniRx;
using UnityEngine;

namespace Game.Upgrades
{
    public class UpgradeController
    {
        private readonly UpgradesSelector _upgradesSelector;
        private IUpgradeView _upgradeView;
        private ContextContainer _globalContextContainer;

        [Inject]
        public UpgradeController(UpgradesSelector upgradesSelector, SceneObjectsContainer sceneObjectsContainer)
        {
            _upgradesSelector = upgradesSelector;
            var upgradeView = sceneObjectsContainer.GetObjectsComponent<IUpgradeView>(SceneObject.UpgradeView);
            Init(upgradesSelector, upgradeView);
        }
        private void Init(UpgradesSelector upgradesSelector, IUpgradeView upgradeView)
        {
            _upgradeView = upgradeView;
            upgradesSelector.CurrentUpgrades.Subscribe(upgradeView.SetUpgrades);
            _upgradeView.OnTryBuyUpgrade.SubscribeWithSkip(TryBuyUpgrade);
        }
        private void TryBuyUpgrade(UpgradeData upgradeData)
        {
            upgradeData.ApplyActions(_globalContextContainer);
            _upgradeView.ApplyUpgradeCallback(upgradeData, true);
        }
        public void SelectNewUpgrades(int upgradesCount)
        {
            _upgradesSelector.SelectNewUpgrades(upgradesCount);
        }

        public void SetContext(GlobalContextContainer globalContextContainer)
        {
            _globalContextContainer = globalContextContainer.ContextContainer;
        }
    }
}
