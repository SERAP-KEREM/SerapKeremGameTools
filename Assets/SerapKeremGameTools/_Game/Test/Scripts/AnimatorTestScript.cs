using UnityEngine;

namespace SerapKeremGameTools._Game._TopDownCharacterSystem
{
    public class AnimatorTestScript : MonoBehaviour
    {
        [SerializeField] private TopDownCharacterAnimator _topDownCharacterAnimator;

        private void Update()
        {
            // Test Animations on Input
            if (Input.GetKeyDown(KeyCode.H)) // Hurt
            {
                _topDownCharacterAnimator.PlayHurtAnimation();
                
            }

            if (Input.GetKeyDown(KeyCode.D)) // Dead
            {
                _topDownCharacterAnimator.PlayDeadAnimation();
            }

            if (Input.GetKeyDown(KeyCode.A)) // Attack
            {
                _topDownCharacterAnimator.PlayAttackAnimation();
            }

            if (Input.GetKeyDown(KeyCode.W)) // Win
            {
                _topDownCharacterAnimator.PlayWinAnimation();
            }
        }
    }
}
