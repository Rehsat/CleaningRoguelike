using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarView : MonoBehaviour
{
    [SerializeField] private Image _progressBar;

    public void SetProgress(float currentValue, float goalValue) // в теории можно заменить сразу на fill amount, но пока не мешает - почему бы и нет
    {
        _progressBar.fillAmount = currentValue / goalValue;
    }
}
