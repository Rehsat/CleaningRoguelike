using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class Utils
{
    public static Sequence GetWorkTween(Transform transform, float durationSeconds = 1f, Ease ease = Ease.OutBack)
    {
        var startScale = transform.localScale;
        var startPosition = transform.position;
        var startPositionY = startPosition.y;
        var scale = startScale.y * 0.85f;
        var movePositionY = startPositionY - (startScale.y-scale) / 2f;
        var halfDuration = durationSeconds/2;
        
        var tween = transform.DOScaleY(scale, halfDuration).SetEase(ease);
        var tween2 = transform.DOMoveY(movePositionY, halfDuration).SetEase(ease);
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
    
    public static double GetTimeDifferenceInMinutes(this DateTime time1, DateTime time2)
    {
        TimeSpan difference = time1 - time2;
        return Math.Abs(difference.TotalMinutes);
    }
}
