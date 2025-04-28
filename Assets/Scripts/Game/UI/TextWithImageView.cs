using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TextWithImageView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _image;

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void SetImage(Sprite image)
        {
            _image.sprite = image;
        }
    }
}
