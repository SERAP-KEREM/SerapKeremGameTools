using UnityEngine;

namespace SerapKeremGameTools._Game._TopDownCharacterSystem
{
    /// <summary>
    /// Controls character animations such as movement, attack, hurt, death, and victory in a top-down perspective.
    /// Combines simplicity with modularity for easy integration and scalability.
    /// </summary>
    public class TopDownCharacterAnimator : MonoBehaviour
    {
        [Header("Required Components")]
        [SerializeField, Tooltip("Animator component to control animations.")]
        private Animator _animator;

        [SerializeField, Tooltip("Character controller providing movement and state data.")]
        private TopDownCharacterController _characterController;

        [SerializeField, Tooltip("ScriptableObject for configurable animation parameters.")]
        private TopDownCharacterConfigSO _characterConfig;

        private IAnimationHandler _animationHandler;

        private void Awake()
        {
            // Check required components
            if (_animator == null) Debug.LogError("Animator is missing!");
            if (_characterController == null) Debug.LogError("CharacterController is missing!");
            if (_characterConfig == null) Debug.LogError("CharacterConfigSO is missing!");

            // Initialize Animation Handler
            _animationHandler = new AnimatorHandler(_animator, _characterConfig);
        }

        private void Update()
        {
            // Update movement animations dynamically
            _animationHandler.UpdateMovementAnimation(_characterController.CurrentVelocityMagnitude);
        }

        public void PlayHurtAnimation() => _animationHandler.PlayTriggerAnimation(_characterConfig.HurtParameter);

        public void PlayDeadAnimation() => _animationHandler.PlayTriggerAnimation(_characterConfig.DeadParameter);

        public void PlayAttackAnimation() => _animationHandler.PlayTriggerAnimation(_characterConfig.AttackParameter);

        public void PlayWinAnimation() => _animationHandler.PlayTriggerAnimation(_characterConfig.WinParameter);
    }
}
