using TMPro;
using UnityEngine;

namespace Game.UI.Interactables
{
    public class ProgressBarWithTextView : ProgressBarView
    {
        [SerializeField] private TMP_Text _text;
        protected override void OnProgressSet(float currentValue, float goalValue)
        {
            _text.text = Mathf.RoundToInt(goalValue - currentValue).ToString(); //TODO добавить режимы создания текста если понадобится
        }
    }
}