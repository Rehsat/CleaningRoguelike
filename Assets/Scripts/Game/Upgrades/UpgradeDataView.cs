using System;
using DG.Tweening;
using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Upgrades
{
    public class UpgradeDataView : MonoBehaviour, IViewWithImage, IViewWithText, IPointerEnterHandler, IPointerExitHandler
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
            SetPrice(upgradeData.ConfigData.Cost);
        }

        private void SendBuyCallback()
        {
            Debug.LogError(4444);
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

        public void SetPrice(float price)
        {
            _price.text = $"{price}$";
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(SendBuyCallback);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(1.2f, 0.3f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, 0.3f);
        }
    }
}