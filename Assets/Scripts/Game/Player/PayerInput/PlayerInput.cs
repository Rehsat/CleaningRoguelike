using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using UnityEngine;

namespace Game.Player.PayerInput
{
    public class PlayerInput
    {
        private Input _input;
        
        private readonly ReactiveEvent<Vector2> _onLookPerformed;
        private readonly ReactiveEvent<bool> _onRunningStateChange;
        private readonly ReactiveTrigger _onInteractButtonPressed;
        private readonly ReactiveEvent<bool> _onThrowButtonPressStateChange;

        public IReadOnlyReactiveEvent<Vector2> OnLookPerformed => _onLookPerformed;
        public IReadOnlyReactiveEvent<bool> OnRunningStateChange => _onRunningStateChange;
        public IReadOnlyReactiveTrigger OnInteractButtonPressed => _onInteractButtonPressed;
        public IReadOnlyReactiveEvent<bool> OnThrowButtonPressStateChange => _onThrowButtonPressStateChange;

        public PlayerInput()
        {
            _input = new Input();
            _onRunningStateChange = new ReactiveEvent<bool>();
            _onLookPerformed = new ReactiveEvent<Vector2>();
            _onInteractButtonPressed = new ReactiveTrigger();
            _onThrowButtonPressStateChange = new ReactiveEvent<bool>();
            
            _input.InputMap.Interact.performed += ctx => _onInteractButtonPressed.Notify();
            
            _input.InputMap.Sprint.performed += ctx => _onRunningStateChange.Notify(true);
            _input.InputMap.Sprint.canceled += ctx => _onRunningStateChange.Notify(false);
            
            _input.InputMap.Look.performed += ctx => 
                _onLookPerformed.Notify(_input.InputMap.Look.ReadValue<Vector2>());

            _input.InputMap.Throw.started += ctx =>_onThrowButtonPressStateChange.Notify(true);
            _input.InputMap.Throw.canceled += ctx => _onThrowButtonPressStateChange.Notify(false);
            _input.Enable();
        }
        
        public Vector2 GetHorizontalMoveDirection()
        {
            var horizontalMovement = _input.InputMap.HorizontalMovement.ReadValue<Vector2>();
            return horizontalMovement;
        }

        public Vector2 GetLook()
        {
            var horizontalMovement = _input.InputMap.Look.ReadValue<Vector2>();
            return horizontalMovement;
        }

    }
}