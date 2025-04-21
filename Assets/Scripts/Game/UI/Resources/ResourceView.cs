using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI.Resources
{
    public class ResourceView : MonoBehaviour, IResourceView
    {
        [SerializeField] private TMP_Text _resourceCountText;
        [SerializeField] private Image _icon; 
        
        public void SetIcon(Sprite icon)
        {
            if (_icon != null)
                _icon.sprite = icon;
        }
        public void UpdateResourceValue(float value, float maxValue)
        {
            _resourceCountText.text = $"{value}/{maxValue}";
        }
    }
}