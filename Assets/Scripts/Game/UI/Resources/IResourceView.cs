using UnityEngine;

namespace Game.UI.Resources
{
    public interface IResourceView
    {
        public void SetIcon(Sprite icon);
        public void UpdateResourceValue(float value, float maxValue);
    }
}