using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO отрефакторить
public class BounceAnimator : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float _delaySeconds;
    [SerializeField] private float jumpHeight = 100f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float fadeDelay = 0.3f;
    [SerializeField] private RectTransform _text;
    [SerializeField] private Canvas _rootCanvas;

    private CanvasGroup _canvasGroup;
    private Vector3 _originalPosition;
    private Vector3 _originalScale;
    private Vector3 _startPosition;
    private bool _animationInProgress;

    private Queue<RectTransform> _transformsToAnimate = new Queue<RectTransform>();

    private void Awake()
    {
       /* _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _originalPosition = _rectTransform.localPosition;
        _originalScale = _rectTransform.localScale;*/
    }

    public void PlayBounceAnimation(RectTransform transformToAnimate, Vector3 position)
    {
        _startPosition = position;
        _transformsToAnimate.Enqueue(transformToAnimate);
        if (_animationInProgress == false)
            StartCoroutine(PlayAnimationSequence());
    }

    private IEnumerator PlayAnimationSequence()
    {
        _animationInProgress = true;
        while (_transformsToAnimate.Count>0)
        {
            PlayBounceAnimation(_transformsToAnimate.Dequeue());
            yield return new WaitForSeconds(_delaySeconds);
        }
        _animationInProgress = false;
    }
    private void PlayBounceAnimation(RectTransform transformToAnimate)
    {
        transformToAnimate.parent = _rootCanvas.transform;
        _canvasGroup = transformToAnimate.GetComponent<CanvasGroup>();

        // Установка позиции и текста
        transformToAnimate.anchoredPosition = _startPosition;
       // GetComponent<Text>().text = transformToAnimate.ToString();

        // Активация объекта
        gameObject.SetActive(true);
        _canvasGroup.alpha = 1f;

        // Создание последовательности анимаций
        Sequence sequence = DOTween.Sequence();


        Tween jump =
            transformToAnimate
                .DOScale(Vector3.one * scaleMultiplier, jumpDuration * 0.5f)
                .SetEase(Ease.OutBack);
        sequence.Append(transformToAnimate
            .DOLocalMoveY(_originalPosition.y + jumpHeight, jumpDuration)
            .SetEase(Ease.OutQuad))
            .Join(jump);
        sequence.Join(transformToAnimate
            .DOLocalMoveX(_originalPosition.x + Random.Range(-120f, 120f), jumpDuration * 1.8f)
            .SetEase(Ease.InOutSine));
        // Падение вниз с затуханием
        sequence.Append(transformToAnimate
            .DOLocalMoveY(_originalPosition.y, jumpDuration * 0.8f)
            .SetEase(Ease.InQuad))
            .Join(_canvasGroup.DOFade(0f, jumpDuration * 0.5f)
            .SetDelay(fadeDelay));

        // Случайное смещение по горизонтали
        sequence.Join(transformToAnimate
            .DOLocalMoveX(_originalPosition.x + Random.Range(-30f, 30f), jumpDuration * 1.8f)
            .SetEase(Ease.InOutSine));

        // Завершение анимации
        sequence.OnComplete(() => transformToAnimate.gameObject.SetActive(false))
            .SetLink(gameObject)
            .Play();
    }
}
