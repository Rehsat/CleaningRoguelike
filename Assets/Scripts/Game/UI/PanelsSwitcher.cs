using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

public class PanelsSwitcher : MonoBehaviour
{
    [SerializeField] private Direction _hideDirection;
    [SerializeField] private SerializableDictionary<Panel, RectTransform> _panels;
    private RectTransform _currentPanel;
    private Vector3 _hideDirectionVector => _hideDirection.ToVector();
    private const float DOWN_VALUE = 650; 
    public void Awake()
    {
        foreach (var rectTransform in _panels.Values)
        {
            rectTransform.transform.position += Vector3.down * DOWN_VALUE;
        }
    }

    public void OpenPanel(Panel panelType)
    {
        Debug.LogError(panelType);
        var movement = _hideDirectionVector * DOWN_VALUE;
        var sequence = DOTween.Sequence();
        if (_currentPanel != null)
        {
            var hideTween = _currentPanel.DOMove(_currentPanel.transform.position - movement, 0.5f);
            sequence.Append(hideTween);
        }
        var panel = _panels[panelType];
        _currentPanel = panel;
        var showTween = panel.DOMove(panel.transform.position + movement, 0.5f).SetEase(Ease.OutBack);
        sequence.Append(showTween);
        sequence.Play();
    }
}
public enum Panel
{
    None = 0,
    Gold = 1,
    Genie = 2
}