using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using UnityEngine;

namespace Game.Player.PayerInput
{
    public class PlayerInput
    {
        private Input _input;
        
        private readonly ReactiveEvent<Vector2> _onLookPerformed;
        private readonly ReactiveEvent<Vector2> _onMousePositionChange;
        private readonly ReactiveEvent<Vector2> _onBuildingRotatePerformed;
        private readonly ReactiveEvent<bool> _onRunningStateChange;
        private readonly ReactiveEvent<bool> _onInteractButtonPressStateChange;
        private readonly ReactiveEvent<bool> _onThrowButtonPressStateChange;
        private readonly ReactiveTrigger _onUpgradesOpenButtonPressed;

        public IReadOnlyReactiveEvent<Vector2> OnLookPerformed => _onLookPerformed;
        public IReadOnlyReactiveEvent<Vector2> OnMousePositionChange => _onMousePositionChange;
        public IReadOnlyReactiveEvent<Vector2> OnBuildingRotatePerformed => _onBuildingRotatePerformed;
        public IReadOnlyReactiveEvent<bool> OnRunningStateChange => _onRunningStateChange;
        public IReadOnlyReactiveEvent<bool> OnInteractButtonPressed => _onInteractButtonPressStateChange;
        public IReadOnlyReactiveEvent<bool> OnThrowButtonPressStateChange => _onThrowButtonPressStateChange;
        public ReactiveTrigger OnUpgradesOpenButtonPressed => _onUpgradesOpenButtonPressed;

        public PlayerInput()
        {
            _input = new Input();
            _onUpgradesOpenButtonPressed = new ReactiveTrigger();
            
            _onLookPerformed = new ReactiveEvent<Vector2>();
            _onBuildingRotatePerformed = new ReactiveEvent<Vector2>();
            _onMousePositionChange = new ReactiveEvent<Vector2>();
            
            _onRunningStateChange = new ReactiveEvent<bool>();
            _onInteractButtonPressStateChange = new ReactiveEvent<bool>();
            _onThrowButtonPressStateChange = new ReactiveEvent<bool>();

            //только сейчас дошло, что можно быыло метод выделить под инит таких вот действий, но уже поздняк, если понадобится - сделаю
            _input.InputMap.Interact.performed += ctx => _onInteractButtonPressStateChange.Notify(true);
            _input.InputMap.Interact.canceled += ctx => _onInteractButtonPressStateChange.Notify(false);
            
            _input.InputMap.Sprint.performed += ctx => _onRunningStateChange.Notify(true);
            _input.InputMap.Sprint.canceled += ctx => _onRunningStateChange.Notify(false);

            _input.InputMap.Throw.started += ctx =>_onThrowButtonPressStateChange.Notify(true);
            _input.InputMap.Throw.canceled += ctx => _onThrowButtonPressStateChange.Notify(false);

            _input.InputMap.ShowUpgrades.performed += ctx => _onUpgradesOpenButtonPressed.Notify();
            
            _input.InputMap.BuildRotate.performed += ctx => 
                _onBuildingRotatePerformed.Notify(_input.InputMap.BuildRotate.ReadValue<Vector2>());
            _input.InputMap.MousePosition.performed += ctx =>
                _onMousePositionChange.Notify(_input.InputMap.MousePosition.ReadValue<Vector2>());
            
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