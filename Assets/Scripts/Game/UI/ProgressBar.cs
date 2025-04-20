using System.Collections;
using System.Collections.Generic;
using Game.Player.Data;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

//TODO УБРАТЬ ПОВТОР
public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _image;

    [Inject]
    public void Construct(ObjectHolder objectHolder)
    {
        objectHolder.CurrentThrowPower.Subscribe(value =>
        {
            gameObject.SetActive(value > 0.00001);
            _image.fillAmount = value;
        });
    }
}
