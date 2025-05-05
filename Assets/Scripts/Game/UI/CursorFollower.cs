using System.Collections;
using System.Collections.Generic;
using Game.Player.PayerInput;
using UnityEngine;
using Zenject;

public class CursorFollower : MonoBehaviour
{
    [SerializeField] private RectTransform targetTransform;
    [SerializeField] private Canvas parentCanvas;
    

    [Inject]
    private void Construct(PlayerInput playerInput)
    {
        if (targetTransform == null)
            targetTransform = GetComponent<RectTransform>();
        
        if (parentCanvas == null)
            parentCanvas = GetComponentInParent<Canvas>();

        playerInput.OnMousePositionChange.SubscribeWithSkip(UpdatePosition);
    }

    private void UpdatePosition(Vector2 screenPosition)
    {
        if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            targetTransform.position = screenPosition;
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                screenPosition,
                parentCanvas.worldCamera,
                out Vector2 localPoint);
            
            targetTransform.localPosition = localPoint;
        }
    }
}
