using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAnimator : MonoBehaviour
{
    [Header("Swing Settings")]
    [SerializeField] [Range(0.1f, 5f)] private float _swingSpeed = 1f;
    [SerializeField] [Range(1f, 90f)] private float _swingAngle = 15f;
    [SerializeField] private bool _startAutomatically = true;

    private RectTransform _transform;
    private float _timeCounter;
    private bool _isSwinging;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
        _isSwinging = _startAutomatically;
    }

    private void Update()
    {
        if (!_isSwinging) return;
        
        UpdateSwingAnimation();
    }

    private void UpdateSwingAnimation()
    {
        _timeCounter += Time.deltaTime * _swingSpeed;
        float angle = Mathf.Sin(_timeCounter) * _swingAngle;
        _transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void StartSwing()
    {
        _isSwinging = true;
        _timeCounter = 0f;
    }

    public void StopSwing()
    {
        _isSwinging = false;
        ResetRotation();
    }

    private void ResetRotation()
    {
        _transform.rotation = Quaternion.identity;
    }

    private void OnDisable()
    {
        if (_isSwinging)
        {
            ResetRotation();
        }
    }
}
