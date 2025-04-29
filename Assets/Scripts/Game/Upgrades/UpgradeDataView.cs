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

        public void SetImage(Sprite image)
        {
            _upgradeIcon.sprite = image;
        }

        public void SetText(string text)
        {
            _title.text = text;
        }
    }
}