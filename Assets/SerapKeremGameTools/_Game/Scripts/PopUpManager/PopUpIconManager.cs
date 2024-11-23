using UnityEngine;
using SerapKeremGameTools._Game._objectPool;
using System.Collections;
using SerapKeremGameTools._Game._Singleton;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Manages the pop-up icon animations and pooling system.
    /// </summary>
    public class PopUpIconManager : MonoSingleton<PopUpIconManager>
    {
        [Header("Pop-Up Settings")]
        [SerializeField, Tooltip("The prefab for the pop-up sprite (using SpriteRenderer).")]
        private PopUpIcon popUpIconPrefab;

        [SerializeField, Tooltip("The initial pool size for pop-up sprites.")]
        private int poolSize = 10;

        [Header("Animation Settings")]
        [SerializeField, Tooltip("The delay before the pop-up sprite disappears.")]
        private float hideDelay = 1f;

        [SerializeField, Tooltip("Duration of the animation.")]
        private float animationDuration = 0.5f;

        [SerializeField, Tooltip("The height the pop-up sprite will bounce.")]
        private float bounceHeight = 2f;

        [SerializeField, Tooltip("The number of times the pop-up sprite will bounce.")]
        private int bounceCount = 3;

        [SerializeField, Tooltip("The offset used for the sliding animation.")]
        private Vector3 slideOffset = new Vector3(0, 2, 0);

        private ObjectPool<PopUpIcon> popUpSpritePool;

        protected override void Awake()
        {
            base.Awake();
            // Create Object Pool for PopUpSpriteRenderer
            popUpSpritePool = new ObjectPool<PopUpIcon>(popUpIconPrefab, poolSize, transform);
        }

        /// <summary>
        /// Displays a pop-up sprite with an animation.
        /// </summary>
        /// <param name="position">The position where the pop-up will appear.</param>
        /// <param name="sprite">The sprite to display.</param>
        /// <param name="customDuration">Custom duration for the animation.</param>
        /// <param name="animationType">The type of animation to use.</param>
        public void ShowPopUpSprite(Vector3 position, Sprite sprite, float customDuration, PopUpAnimationType animationType)
        {
            // Get a PopUpSpriteRenderer from the pool
            PopUpIcon popUpSprite = popUpSpritePool.GetObject();

            // Set the position of the sprite
            popUpSprite.transform.position = position;
            // Initialize the sprite's visual properties
            popUpSprite.Initialize(sprite);

            // Determine the duration for the animation
            float duration = customDuration > 0 ? customDuration : animationDuration;

            // Start animation coroutine
            StartCoroutine(HandleAnimation(popUpSprite, duration, animationType));
            // After the delay, return the object to the pool
            StartCoroutine(ReturnPopUpSpriteAfterDelay(popUpSprite, duration + hideDelay));
        }

        /// <summary>
        /// Handles the animation based on the selected type.
        /// </summary>
        /// <param name="popUpSprite">The pop-up sprite to animate.</param>
        /// <param name="duration">Duration of the animation.</param>
        /// <param name="animationType">The type of animation to apply.</param>
        private IEnumerator HandleAnimation(PopUpIcon popUpSprite, float duration, PopUpAnimationType animationType)
        {
            switch (animationType)
            {
                case PopUpAnimationType.ScaleAndFade:
                    yield return ScaleAndFadeAnimation(popUpSprite, duration);
                    break;
                case PopUpAnimationType.SlideUp:
                    yield return SlideAnimation(popUpSprite, slideOffset, duration);
                    break;
                case PopUpAnimationType.SlideDown:
                    yield return SlideAnimation(popUpSprite, -slideOffset, duration);
                    break;
                case PopUpAnimationType.Bounce:
                    yield return BounceAnimation(popUpSprite, duration);
                    break;
            }
        }

        /// <summary>
        /// Handles the scale and fade animation of the pop-up sprite.
        /// </summary>
        /// <param name="popUpSprite">The pop-up sprite to animate.</param>
        /// <param name="duration">Duration of the animation.</param>
        private IEnumerator ScaleAndFadeAnimation(PopUpIcon popUpSprite, float duration)
        {
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float elapsedTime = 0f;

            // Animate scaling effect
            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                popUpSprite.transform.localScale = Vector3.Lerp(startScale, endScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Wait for the specified duration before reversing the animation
            yield return new WaitForSeconds(duration);

            elapsedTime = 0f;
            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                popUpSprite.transform.localScale = Vector3.Lerp(endScale, startScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Handles the sliding animation of the pop-up sprite.
        /// </summary>
        /// <param name="popUpSprite">The pop-up sprite to animate.</param>
        /// <param name="offset">The direction and distance to slide.</param>
        /// <param name="duration">Duration of the animation.</param>
        private IEnumerator SlideAnimation(PopUpIcon popUpSprite, Vector3 offset, float duration)
        {
            Vector3 startPosition = popUpSprite.transform.position;
            Vector3 targetPosition = startPosition + offset;
            float elapsedTime = 0f;

            // Animate sliding effect
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = Mathf.SmoothStep(0, 1, t);
                popUpSprite.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            popUpSprite.transform.position = targetPosition;
        }

        /// <summary>
        /// Handles the bounce animation of the pop-up sprite.
        /// </summary>
        /// <param name="popUpSprite">The pop-up sprite to animate.</param>
        /// <param name="duration">Duration of the animation.</param>
        private IEnumerator BounceAnimation(PopUpIcon popUpSprite, float duration)
        {
            Vector3 startPosition = popUpSprite.transform.position;
            float elapsedTime = 0f;

            // Animate bouncing effect
            for (int i = 0; i < bounceCount; i++)
            {
                float t = 0f;
                while (t < 1f)
                {
                    float yOffset = Mathf.Sin(t * Mathf.PI) * bounceHeight;
                    popUpSprite.transform.position = startPosition + new Vector3(0, yOffset, 0);
                    t += Time.deltaTime / (duration / bounceCount);
                    yield return null;
                }
            }

            popUpSprite.transform.position = startPosition;
        }

        /// <summary>
        /// Returns the pop-up sprite to the pool after a specified delay.
        /// </summary>
        /// <param name="popUpIcon">The pop-up icon to return.</param>
        /// <param name="delay">The time to wait before returning.</param>
        private IEnumerator ReturnPopUpSpriteAfterDelay(PopUpIcon popUpIcon, float delay)
        {
            yield return new WaitForSeconds(delay);
            // Return the sprite to the pool after animation is done
            ReturnPopUpSprite(popUpIcon);
        }

        /// <summary>
        /// Returns a pop-up sprite to the pool.
        /// </summary>
        /// <param name="popUpIcon">The pop-up icon to return.</param>
        public void ReturnPopUpSprite(PopUpIcon popUpIcon)
        {
            // Reset the sprite properties if needed before returning
            popUpIcon.ResetProperties();
            // Return the sprite to the pool
            popUpSpritePool.ReturnObject(popUpIcon);
        }

        /// <summary>
        /// Retrieves a pop-up sprite renderer from the object pool.
        /// </summary>
        /// <returns>A PopUpIcon from the pool.</returns>
        public PopUpIcon GetPopUpSpriteRenderer()
        {
            // Get an object from the pool
            return popUpSpritePool.GetObject();
        }

    }
}
