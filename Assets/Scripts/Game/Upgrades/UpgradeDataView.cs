using System;
using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Upgrades
{
    public class UpgradeDataView : MonoBehaviour, IViewWithImage, IViewWithText
    {
        [SerializeField] private Image _upgradeIcon;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private Button _buyButton;

        private UpgradeData _myUpgradeData;
        private ReactiveEvent<UpgradeData> _onBuy;
        public ReactiveEvent<UpgradeData> OnBuy => _onBuy;

        public void Construct(UpgradeData upgradeData)
        {
            _onBuy = new ReactiveEvent<UpgradeData>();
            _myUpgradeData = upgradeData;
            _buyButton.onClick.AddListener(SendBuyCallback);
            SetText(upgradeData.ConfigData.Name);
            SetImage(upgradeData.ConfigData.Icon);
        }

        private void SendBuyCallback()
        {
            _onBuy.Notify(_myUpgradeData);
        }
        public void SetImage(Sprite image)
        {
            _upgradeIcon.sprite = image;
        }

        public void SetText(string text)
        {
            _title.text = text;
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(SendBuyCallback);
        }
    }
}