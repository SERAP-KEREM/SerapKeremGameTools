using UnityEngine;

namespace SerapKeremGameTools._Game._TopDownCharacterSystem
{
    /// <summary>
    /// Handles animation updates and triggers for the Animator component.
    /// </summary>
    public class AnimatorHandler : IAnimationHandler
    {
        private readonly Animator _animator;
        private readonly TopDownCharacterConfigSO _config;

        // Cached hash codes for efficiency
        private readonly int _speedHash;
        private readonly int _hurtHash;
        private readonly int _deadHash;
        private readonly int _attackHash;
        private readonly int _winHash;

        public AnimatorHandler(Animator animator, TopDownCharacterConfigSO config)
        {
            _animator = animator;
            _config = config;

            // Precompute hash codes for performance
            _speedHash = Animator.StringToHash(_config.SpeedParameter);
            _hurtHash = Animator.StringToHash(_config.HurtParameter);
            _deadHash = Animator.StringToHash(_config.DeadParameter);
            _attackHash = Animator.StringToHash(_config.AttackParameter);
            _winHash = Animator.StringToHash(_config.WinParameter);
        }

        public void UpdateMovementAnimation(float speed)
        {
            _animator.SetFloat(_speedHash, speed);
        }

        public void PlayTriggerAnimation(string parameterName)
        {
            int parameterHash = Animator.StringToHash(parameterName);
            _animator.SetTrigger(parameterHash);
        }
    }
}
