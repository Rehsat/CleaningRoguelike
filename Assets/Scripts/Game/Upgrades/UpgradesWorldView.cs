using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Player.PayerInput;
using UniRx;
using UnityEngine;
using Zenject;

public class UpgradesWorldView : MonoBehaviour
{
    [SerializeField] private Transform _rootTransform;
    [SerializeField] private float _showRotation = 90;
    [SerializeField] private float _hideRotation = -20;
    [SerializeField] private float _animationDuration = 0.5f;
    private bool _isAnimationInProgress;
    private ReactiveProperty<bool> _isUpgradeShowed;

    [Inject]
    public void Construct(PlayerInput playerInput)
    {
        _isUpgradeShowed = new ReactiveProperty<bool>();
        playerInput.OnUpgradesOpenButtonPressed.SubscribeWithSkip((() =>
        {
            if (_isAnimationInProgress) return;
            _isUpgradeShowed.Value = !_isUpgradeShowed.Value;
        }));

        var isFirstTime = true;
        _isUpgradeShowed.Subscribe((isShowing =>
        {
            if (isFirstTime)
            {
                isFirstTime = false;
                return;
            }
            
            _isAnimationInProgress = true;
            
            var resultRotation = isShowing ? _showRotation : _hideRotation;
            var startRotation = isShowing ? _hideRotation : _showRotation;
            var ease = isShowing ? Ease.OutBack : Ease.InBack;
            _rootTransform
                .DOLocalRotate(new Vector3(startRotation, _rootTransform.localEulerAngles.y), 0);
            _rootTransform
                .DOLocalRotate(new Vector3(resultRotation, _rootTransform.localRotation.y), _animationDuration)
                .SetEase(ease)
                .OnComplete((() => _isAnimationInProgress = false));
        }));
    }
}
