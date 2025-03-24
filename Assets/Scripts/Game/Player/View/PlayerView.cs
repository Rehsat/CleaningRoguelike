using DG.Tweening;
using Game.Player.Data;
using Game.Player.PayerInput;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float lookSpeed = 2f;
        [SerializeField] private float _throwPowerCollectSpeed;
        [SerializeField] private float _throwPowerScale;
        [SerializeField] private Transform playerCamera;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Transform _holdObjectPosition;
        [SerializeField] private HeadBob _headBob;
        [SerializeField] private PlayerInteractablesDetector _playerInteractablesDetector;

        private float verticalRotation = 0f;
        private float _speedMultiplier;
        private float _startFieldOfView;
        
        private ReactiveProperty<bool> _isRunning;
        private PlayerInput _playerInput;
        private ObjectHolder _objectHolder;
        
        [Inject]
        public void Construct(PlayerInput playerInput, ObjectHolder objectHolder)
        {
            _isRunning = new ReactiveProperty<bool>();
            _playerInput = playerInput;
            _objectHolder = objectHolder;
            _startFieldOfView = _playerCamera.fieldOfView;

            _headBob.Construct(_isRunning);
            _playerInteractablesDetector.Construct(_playerCamera, playerInput, objectHolder);
            
            _playerInput.OnRunningStateChange.SubscribeWithSkip(isRunning => _isRunning.Value = isRunning);
            _isRunning.Subscribe(isRunning =>
            {
                _speedMultiplier = isRunning ? 2 : 1;
            });
            InitObjectHolder(objectHolder);
        }

        private void InitObjectHolder(ObjectHolder objectHolder)
        {
            objectHolder.CurrentPickedUpObject.Subscribe((rigidbody1 =>
            {
                if(rigidbody1 == null) return;
                rigidbody1.transform.parent = _holdObjectPosition;
                rigidbody1.transform.DOLocalMove(Vector3.zero, 0.25f);
                rigidbody1.transform.DOLocalRotate(Vector3.zero, 0.25f);
            }));
            _playerInput.OnThrowButtonPressStateChange.SubscribeWithSkip(isPressed =>
            {
                if (isPressed)
                {
                    objectHolder.StartThrowing(_throwPowerCollectSpeed);
                }
                else
                {
                    var holdableTransform = objectHolder.CurrentPickedUpObject.Value.transform;
                    holdableTransform.parent = null;
                    holdableTransform.position += Vector3.up;
                    objectHolder.Throw(_throwPowerScale, _playerCamera.transform.forward + _characterController.velocity/20);
                }
            });
        }

        private void Update()
        {
            Look(_playerInput.GetLook() * Time.deltaTime);
            CharacterMove();
        }

        private void Look(Vector2 lookInput)
        {
            float mouseX = lookInput.x * lookSpeed;
            float mouseY = lookInput.y * lookSpeed;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -75f, 75f);

            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
            transform.Rotate(Vector3.up * mouseX);
        }
        
        private void CharacterMove()
        {
            var direction = _playerInput.GetHorizontalMoveDirection();
            var fieldOfView = direction.magnitude > 0 && _isRunning.Value ? _startFieldOfView + 5 : _startFieldOfView;
            _playerCamera.fieldOfView = Mathf.Lerp(_playerCamera.fieldOfView, fieldOfView, 5 * Time.deltaTime);
                
            var forward = transform.forward;
            var right = transform.right;
            forward.Normalize();
            right.Normalize();
            
            Vector3 move = forward * direction.y + right * direction.x;
            var horizontalMovement = move * _speed * _speedMultiplier;
            
            var gravity = Vector3.up * -25f * Time.deltaTime;
            var velocity = new Vector3(horizontalMovement.x, gravity.y, horizontalMovement.z);
            _characterController.Move(velocity * Time.deltaTime);
        }
        private void OnValidate()
        {
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();
        }
    }
}
