using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI.Resources
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resourceCountText;
        
        [Inject]
        public void Construct()
        {
            
        }

        public void SetIcon()
        {
            
        }
        public void UpdateResourceValue(float value, float maxValue)
        {
            _resourceCountText.text = $"{value}/{maxValue}";
        }
    }
}