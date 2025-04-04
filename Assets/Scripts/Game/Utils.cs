using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class Utils
{
    public static Sequence GetWorkTween(Transform transform, float durationSeconds = 1f)
    {
        var startScale = transform.localScale;
        var startPosition = transform.position;
        var startPositionY = startPosition.y;
        var scale = startScale.y * 0.85f;
        var movePositionY = startPositionY - (startScale.y-scale) / 2f;
        var halfDuration = durationSeconds/2;
        
        var tween = transform.DOScaleY(scale, halfDuration).SetEase(Ease.OutBack);
        var tween2 = transform.DOMoveY(movePositionY, halfDuration).SetEase(Ease.OutBack);
        return DOTween.Sequence()
            .Append(tween)
            .Join(tween2)
            .SetLoops(-1, LoopType.Yoyo)
            .OnKill(() =>
            {
                transform.localScale = startScale;
                transform.position = startPosition;
            });
    }
}
