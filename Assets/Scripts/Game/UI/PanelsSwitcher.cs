using DG.Tweening;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

namespace Game.UI
{
    public class PanelsSwitcher : MonoBehaviour, IGameStateChangeView
    {
        //TODO добавить дженерик для энама
        [SerializeField] private Direction _hideDirection;
        [SerializeField] private SerializableDictionary<GameState, RectTransform> _panels;
        private RectTransform _currentPanel;
        private Vector3 _hideMovement => _hideDirection.ToVector() * DOWN_VALUE;
        private const float DOWN_VALUE = 650; 
        public void Awake()
        {
            foreach (var rectTransform in _panels.Values)
            {
                rectTransform.transform.localPosition += _hideMovement;
            }
        }

        public void OpenPanel(GameState panelType)
        {
            var sequence = DOTween.Sequence();
            if (_currentPanel != null)
            {
                var currentPanel = _currentPanel;
                var hideTween = _currentPanel
                    .DOLocalMove(_currentPanel.transform.localPosition + _hideMovement, 0.5f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => currentPanel.gameObject.SetActive(false));
                
                sequence
                    .Append(hideTween);
                
                _currentPanel = null;
            }
            if (_panels.ContainsKey(panelType))
            {
                var panel = _panels[panelType];
                _currentPanel = panel;
                _currentPanel.gameObject.SetActive(true);
                
                var showPosition = panel.transform.localPosition - _hideMovement;
                var showTween = panel
                    .DOLocalMove(showPosition, 0.5f)
                    .SetEase(Ease.OutBack);
                sequence.Append(showTween);
            }
            sequence.Play();
        }

        public void OnGameStateChanged(GameState currentState)
        {
            OpenPanel(currentState);
        }
    }
}