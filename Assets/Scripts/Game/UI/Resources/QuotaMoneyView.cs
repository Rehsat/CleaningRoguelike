using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI.Resources
{
    public class QuotaMoneyView : MonoBehaviour, IResourceView
    {
        [SerializeField] private TMP_Text _resourceCountText;
        
        [Inject]
        public void Construct()
        {
            
        }
        public void UpdateResourceValue(float value)
        {
            _resourceCountText.text = $"{value}/99999";
        }
    }
}