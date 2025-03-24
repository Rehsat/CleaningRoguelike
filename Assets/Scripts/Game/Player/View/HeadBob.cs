using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool _enable = true;

    [SerializeField] private bool _test;
    [SerializeField, Range(0, 0.1f)] private float _Amplitude = 0.015f; 
    [SerializeField, Range(0, 30)] private float _frequency = 10.0f;
    [SerializeField] private Transform _camera = null; 
    [SerializeField] private Transform _cameraHolder = null;

    private float _toggleSpeed = 1.0f;
    private float _multiplier = 1.0f;
    private Vector3 _startPos;
    private CharacterController _controller;

    public void Construct(ReactiveProperty<bool> isRunning)
    {
        _controller = GetComponent<CharacterController>();
        _startPos = _camera.localPosition;
        isRunning.Subscribe(value => _multiplier = value ? 1 : 0.6f); 
    }

    void Update()
    {
        if (!_enable) return;
        if(CheckMotion())
            PlayMotion(FootStepMotion());
        else
            ResetPosition();
        _camera.LookAt(FocusTarget());
    }
    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency * _multiplier) * _Amplitude * _multiplier * _multiplier;
        //pos.x += Mathf.Cos(Time.time * _frequency / 10) * _Amplitude;
        return pos;
    }
    private bool CheckMotion()
    {
        float speed = new Vector3(_controller.velocity.x, 0, _controller.velocity.z).magnitude;
        if (speed < _toggleSpeed) return false;
        if (_controller.isGrounded == false) return false;
        return true;
    }
    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion * Time.deltaTime * 100;
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }
    private void ResetPosition()
    {
        if (_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, 2.5f * Time.deltaTime);
    }
}
