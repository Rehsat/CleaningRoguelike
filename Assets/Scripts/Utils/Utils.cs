using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class Utils
{
    //TODO ОТСОРТИРОВАТЬ ПО РАЗНЫМ ФАЙЛАМ
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

    public static Vector3 ToVector(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector3.up;
            case Direction.Down:
                return Vector3.down;
            case Direction.Left:
                return Vector3.left;
            case Direction.Right:
                return Vector3.right;
        }
        throw new ArgumentOutOfRangeException();
    }
    public static void DoShowAnimation(this Transform transform, float secondsDuration = 0.3f, bool withOriginalScale = false)
    {
        transform.gameObject.SetActive(true);
        var originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        if (withOriginalScale)
            transform.DOScale(originalScale, secondsDuration).SetEase(Ease.OutBack);
        else
            transform.DOScale(1f, secondsDuration).SetEase(Ease.OutBack);
    }
    public static void DoHideAnimation(this Transform transform, float secondsDuration = 0.3f)
    {
        transform.DOScale(0, secondsDuration).SetEase(Ease.InBack)
            .OnComplete((() => transform.gameObject.SetActive(false)));
    }
    public static Vector3 SnapToGrid(this Vector3 vector3, float gridSize = 1.0f)
    {
        return new Vector3(
            Mathf.Round(vector3.x / gridSize) * gridSize,
            Mathf.Round(vector3.y / gridSize) * gridSize,
            Mathf.Round(vector3.z / gridSize) * gridSize);
    }
    
}
