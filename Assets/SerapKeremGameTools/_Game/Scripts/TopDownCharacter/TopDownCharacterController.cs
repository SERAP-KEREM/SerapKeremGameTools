using SerapKeremGameTools._Game._InputSystem;
using System.Collections;
using UnityEngine;

namespace SerapKeremGameTools._Game._TopDownCharacterSystem
{
    /// <summary>
    /// Top-down character controller that handles the movement, rotation, gravity, and speed boost of the character.
    /// </summary>
    public class TopDownCharacterController : MonoBehaviour
    {
        [SerializeField, Tooltip("The component responsible for handling the character's physical movement.")]
        private CharacterController _characterController;

        [SerializeField, Tooltip("Configuration containing the movement parameters for the character.")]
        private TopDownCharacterConfigSO _characterConfig;

        [SerializeField, Tooltip("Custom gravity value that can be used to override the default gravity.")]
        private float _customGravity = -9.81f;

        private Vector3 _currentVelocity; // Movement vector
        private Vector3 _verticalVelocity; // Vertical movement vector
        private float _speedBoostMultiplier = 1f; // Speed boost multiplier

        private Coroutine _pauseMovementCoroutine;

        /// <summary>
        /// Returns the magnitude of the character's current velocity.
        /// </summary>
        public float CurrentVelocityMagnitude => _currentVelocity.magnitude;

        private void Awake()
        {
            // Check if the required components are assigned
#if UNITY_EDITOR
            if (_characterController == null)
                Debug.LogError("CharacterController reference not assigned!");

            if (PlayerInput.Instance == null)
                Debug.LogError("PlayerInput reference not assigned!");
#endif
        }

        private void OnEnable()
        {
            PlayerInput.Instance.OnMousePositionInput.AddListener(UpdateCharacterRotation);
        }

        private void OnDisable()
        {
            PlayerInput.Instance.OnMousePositionInput.RemoveListener(UpdateCharacterRotation);
        }

        private void Update()
        {
            // Handle movement and gravity
            Vector2 input = PlayerInput.Instance.GetMovementInput(); // Get input from the player
            HandleMovementInput(input);
            ApplyGravity();
        }

        /// <summary>
        /// Processes the movement input and applies acceleration or deceleration accordingly.
        /// </summary>
        public void HandleMovementInput(Vector2 input)
        {
            Vector3 movementDirection = new Vector3(input.x, 0, input.y).normalized;

            if (movementDirection.magnitude >= 0.1f)
            {
                float targetSpeed = _characterConfig.MovementSpeed * _speedBoostMultiplier;
                _currentVelocity = Vector3.MoveTowards(
                    _currentVelocity,
                    movementDirection * targetSpeed,
                    _characterConfig.Acceleration * Time.deltaTime
                );
            }
            else
            {
                _currentVelocity = Vector3.MoveTowards(
                    _currentVelocity,
                    Vector3.zero,
                    _characterConfig.Deceleration * Time.deltaTime
                );
            }

            _characterController.Move(_currentVelocity * Time.deltaTime);
        }

        /// <summary>
        /// Rotates the character towards the mouse position.
        /// </summary>
        public void UpdateCharacterRotation(Vector3 mousePosition)
        {
            Vector3 directionToMouse = mousePosition - transform.position;
            directionToMouse.y = 0;

            if (directionToMouse.sqrMagnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToMouse);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    _characterConfig.RotationSpeed * Time.deltaTime
                );
            }
        }

        /// <summary>
        /// Applies gravity to the character.
        /// </summary>
        private void ApplyGravity()
        {
            if (!_characterController.isGrounded)
            {
                _verticalVelocity.y += _customGravity * Time.deltaTime;
            }
            else
            {
                _verticalVelocity.y = 0;
            }

            _characterController.Move(_verticalVelocity * Time.deltaTime);
        }

        /// <summary>
        /// Pauses the character's movement for a specified duration.
        /// </summary>
        public void PauseMovement(float duration)
        {
            if (_pauseMovementCoroutine != null)
                StopCoroutine(_pauseMovementCoroutine);

            _pauseMovementCoroutine = StartCoroutine(PauseMovementCoroutine(duration));
        }

        private IEnumerator PauseMovementCoroutine(float duration)
        {
            _currentVelocity = Vector3.zero;
            _verticalVelocity = Vector3.zero;
            yield return new WaitForSeconds(duration);
        }

        /// <summary>
        /// Boosts the character's speed by a specified percentage for a given duration.
        /// </summary>
        public void BoostSpeed(float percentageIncrease, float duration)
        {
            _speedBoostMultiplier = 1 + (percentageIncrease / 100);
            StartCoroutine(ResetSpeedBoostAfterTime(duration));
        }

        private IEnumerator ResetSpeedBoostAfterTime(float duration)
        {
            yield return new WaitForSeconds(duration);
            _speedBoostMultiplier = 1f;
        }
    }
}
