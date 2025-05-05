using Game.Services;
using Gasme.Configs;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.UI.Resources
{
    public class GameValueChangeObserver 
    {
        private readonly BounceAnimator _bounceAnimator;
        private readonly PrefabsContainer _prefabsContainer;
        private CompositeDisposable _compositeDisposable;

        [Inject]
        public GameValueChangeObserver(GameValuesContainer gameValues, BounceAnimator bounceAnimator, PrefabsContainer prefabsContainer)
        {
            _compositeDisposable = new CompositeDisposable();
            _bounceAnimator = bounceAnimator;
            _prefabsContainer = prefabsContainer;
            StartObserveValue(gameValues.GetPlayerValue(PlayerValue.QuotaMoney));
        }

        public void StartObserveValue(PlayerGameValueData gameValueData)
        {
            var textWithImagePrefab = _prefabsContainer.GetPrefabsComponent<TextWithImageView>(Prefab.TextWithImage);
            var resourceView = Object.Instantiate(textWithImagePrefab);
            resourceView.SetImage(gameValueData.Config.Icon);
            gameValueData.OnValueChanged.SubscribeWithSkip((valueChange =>
            {
                var text = valueChange < 0 ? valueChange.ToString() : $"+{valueChange}";
                resourceView.SetText(text);
                _bounceAnimator.PlayBounceAnimation(resourceView.GetComponent<RectTransform>(), Vector3.zero);
            }));
        }
    }
}
