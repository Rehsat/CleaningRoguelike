using System.Collections;
using System.Collections.Generic;
using Game.Player.PayerInput;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CursorImage : MonoBehaviour
{
    [SerializeField] private Image _cursorImage; 
    [Inject]
    public void Construct(PlayerInput input)
    {
        input.OnInteractButtonPressed.SubscribeWithSkip(isPressed =>
        {
            var scale = isPressed ? 0.7f : 1f;
            _cursorImage.transform.localScale = scale * Vector3.one;
        });
    }
}
