using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Game.Interactables
{
    public class InteractableButton : InteractableView
    {
        [SerializeField] private Interaction _interactionToAnimate;
        [SerializeField] private float _scaleOnPress = 0.75f;
        [SerializeField] private float _secondsToAnimate = 0.3f;
        private float? _startScale;
        private Sequence _sequence;
        
        protected override void OnInteract(ContextContainer context, Interaction interactionType)
        {
            if(_interactionToAnimate != interactionType) return;
            
            if (_startScale == null)
                _startScale = transform.localScale.z;
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (float)_startScale);

            _sequence.Append(transform.DOScaleZ((float)_startScale * _scaleOnPress,_secondsToAnimate )
                .SetLoops(2, LoopType.Yoyo));
            _sequence.Play();
        }
    }
}