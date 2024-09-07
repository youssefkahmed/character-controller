using System;
using System.Collections;
using System.Collections.Generic;
using CharacterControllers.Input;
using UnityEngine;

namespace CharacterControllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Components:")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Animator animator;
        
        [Header("Movement Values:")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float rotationSpeed;
        
        [Header("Jumping Values:")]
        [SerializeField] private float maxJumpHeight = 0.5f;
        [SerializeField] private float maxJumpTime = 1.0f;
        [SerializeField] private float jumpResetTime = 0.5f;
        
        [Header("Gravity:")]
        [SerializeField] private float gravity = -9.8f;
        [SerializeField] private float groundedGravity = -0.05f;
        [SerializeField] private float maxFallSpeed = -20f;
        [SerializeField] private float fallMultiplier = 2f;
        
        private CharacterController _characterController;
        
        private Vector2 _moveInput;
        private Vector3 _currentVelocity = Vector3.zero;
        private Vector3 _appliedVelocity = Vector3.zero;
        private bool _isMovePressed;

        private float _currentSpeed;
        private bool _isRunning;

        private Dictionary<int, float> _initialJumpVelocities;
        private Dictionary<int, float> _jumpGravities;
        private float _initialJumpVelocity;
        private bool _isJumping;
        private bool _isJumpPressed;
        private bool _isJumpAnimating;
        private int _jumpCount;
        private Coroutine _jumpResetRoutine;
        
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private static readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
        private static readonly int JumpCountHash = Animator.StringToHash("JumpCount");

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            
            _currentSpeed = walkSpeed;

            float timeToApex = maxJumpTime / 2;
            gravity = -2 * maxJumpHeight / Mathf.Pow(timeToApex, 2);
            _initialJumpVelocity = 2 * maxJumpHeight / timeToApex;
            
            float secondJumpGravity = -2 * (maxJumpHeight + 2) / Mathf.Pow(timeToApex * 1.25f, 2);
            float secondJumpVelocity = 2 * (maxJumpHeight + 2) / (timeToApex * 1.25f);
            
            float thirdJumpGravity = -2 * (maxJumpHeight + 4) / Mathf.Pow(timeToApex * 1.5f, 2);
            float thirdJumpVelocity = 2 * (maxJumpHeight + 4) / (timeToApex * 1.5f);

            _initialJumpVelocities = new Dictionary<int, float>
            {
                {1, _initialJumpVelocity},
                {2, secondJumpVelocity},
                {3, thirdJumpVelocity}
            };

            _jumpGravities = new Dictionary<int, float>
            {
                {0, gravity},
                {1, gravity},
                {2, secondJumpGravity},
                {3, thirdJumpGravity}
            };
        }

        private void Start()
        {
            inputReader.EnablePlayerActions();
            inputReader.Move += ReadMoveInput;
            inputReader.Run += ReadRunInput;
            inputReader.Jump += ReadJumpInput;
        }

        private void Update()
        {
            UpdateAnimator();
            
            HandleRotation();
            MoveCharacter();
            
            HandleGravity();
            HandleJump();
        }

        private void MoveCharacter()
        {
            _appliedVelocity.x = _currentVelocity.x;
            _appliedVelocity.z = _currentVelocity.z;
            _characterController.Move(_currentSpeed * Time.deltaTime * _appliedVelocity);
        }
        
        private void HandleRotation()
        {
            var positionToLookAt = new Vector3(_currentVelocity.x, 0.0f, _currentVelocity.z);

            Quaternion currentRotation = transform.rotation;
            if (_isMovePressed)
            {
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        
        private void HandleGravity()
        {
            bool isFalling = _currentVelocity.y <= 0.0f || !_isJumpPressed;
            
            float previousYVelocity = _currentVelocity.y;
            _currentVelocity.y = isFalling
                ? _currentVelocity.y + _jumpGravities[_jumpCount] * fallMultiplier * Time.deltaTime
                : _currentVelocity.y + _jumpGravities[_jumpCount] * Time.deltaTime;

            _currentVelocity.y = _characterController.isGrounded ? groundedGravity : _currentVelocity.y;
            _appliedVelocity.y = _characterController.isGrounded
                ? groundedGravity
                : Mathf.Max((previousYVelocity + _currentVelocity.y) * 0.5f, maxFallSpeed);
        }
        
        private void HandleJump()
        {
            if (_isJumpPressed && !_isJumping && _characterController.isGrounded)
            {
                if (_jumpCount < _initialJumpVelocities.Count && _jumpResetRoutine != null)
                {
                    StopCoroutine(_jumpResetRoutine);
                }
                
                _isJumping = true;
                _jumpCount = Math.Clamp(_jumpCount + 1, 1, _initialJumpVelocities.Count);
                _currentVelocity.y = _initialJumpVelocities[_jumpCount];
                _appliedVelocity.y = _initialJumpVelocities[_jumpCount];
            }
            else if (!_isJumpPressed && _isJumping && _characterController.isGrounded)
            {
                _isJumping = false;
                _jumpResetRoutine = StartCoroutine(JumpResetRoutine());
            }
        }

        private IEnumerator JumpResetRoutine()
        {
            yield return new WaitForSeconds(jumpResetTime);

            _jumpCount = 0;
        }
        
        private void UpdateAnimator()
        {
            Vector3 locomotion = _currentVelocity;
            locomotion.y = 0;
            animator.SetFloat(LocomotionHash, _isRunning ? locomotion.magnitude : locomotion.magnitude * 0.25f);
            
            if (_isJumpPressed && !_isJumping)
            {
                _isJumpAnimating = true;
            }
            else if (!_isJumping)
            {
                _isJumpAnimating = false;
            }
            animator.SetBool(IsJumpingHash, _isJumpAnimating);
            animator.SetInteger(JumpCountHash, _jumpCount);
        }
        
        #region Input Reading

        private void ReadMoveInput(Vector2 velocity)
        {
            _moveInput = velocity;
            _currentVelocity.x = _moveInput.x;
            _currentVelocity.z = _moveInput.y;

            _isMovePressed = _moveInput.x != 0 || _moveInput.y != 0;
        }
        
        private void ReadRunInput(bool isPressed)
        {
            _currentSpeed = isPressed ? runSpeed : walkSpeed;
            _isRunning = isPressed;
        }
        
        private void ReadJumpInput(bool isPressed)
        {
            _isJumpPressed = isPressed;
        }
        
        #endregion
    }
}
