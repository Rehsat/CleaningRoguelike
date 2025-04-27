using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BounceAnimator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float jumpHeight = 100f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float fadeDelay = 0.3f;
    [SerializeField] private RectTransform _text;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector3 _originalPosition;
    private Vector3 _originalScale;

    private void Awake()
    {
       /* _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _originalPosition = _rectTransform.localPosition;
        _originalScale = _rectTransform.localScale;*/
    }

    public void PlayBounceAnimation(RectTransform transformToAnimate, Vector3 position)
    {
        transformToAnimate = _text;
        _rectTransform = transformToAnimate;
        _canvasGroup = transformToAnimate.GetComponent<CanvasGroup>();

        // Установка позиции и текста
        _rectTransform.anchoredPosition = position;
       // GetComponent<Text>().text = transformToAnimate.ToString();

        // Активация объекта
        gameObject.SetActive(true);
        _canvasGroup.alpha = 1f;

        // Создание последовательности анимаций
        Sequence sequence = DOTween.Sequence();


        Tween jump =
            _rectTransform
                .DOScale(Vector3.one * scaleMultiplier, jumpDuration * 0.5f)
                .SetEase(Ease.OutBack);
        sequence.Append(_rectTransform
            .DOLocalMoveY(_originalPosition.y + jumpHeight, jumpDuration)
            .SetEase(Ease.OutQuad))
            .Join(jump);

        // Падение вниз с затуханием
        sequence.Append(_rectTransform
            .DOLocalMoveY(_originalPosition.y, jumpDuration * 0.8f)
            .SetEase(Ease.InQuad))
            .Join(_canvasGroup.DOFade(0f, jumpDuration * 0.5f)
            .SetDelay(fadeDelay));

        // Случайное смещение по горизонтали
        sequence.Join(_rectTransform
            .DOLocalMoveX(_originalPosition.x + Random.Range(-30f, 30f), jumpDuration * 1.8f)
            .SetEase(Ease.InOutSine));

        // Завершение анимации
        sequence.OnComplete(() => gameObject.SetActive(false))
            .SetLink(gameObject)
            .Play();
    }
}
