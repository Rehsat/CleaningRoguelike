using UnityEngine;
using Zenject;

namespace Game.UI.Resources
{
    public class GameValueChangeObserver
    {
        private readonly BounceAnimator _bounceAnimator;

        [Inject]
        public GameValueChangeObserver(GameValuesContainer gameValues, BounceAnimator bounceAnimator)
        {
            Debug.LogError(123);
            _bounceAnimator = bounceAnimator;
            StartObserveValue(gameValues.GetPlayerValue(PlayerValue.QuotaMoney));
        }

        public void StartObserveValue(PlayerGameValueData gameValueData)
        {
            gameValueData.OnValueChanged.SubscribeWithSkip((f =>
            {
                Debug.LogError(f);
                _bounceAnimator.PlayBounceAnimation(null, Vector3.zero);
            }));
        }
    }
}
