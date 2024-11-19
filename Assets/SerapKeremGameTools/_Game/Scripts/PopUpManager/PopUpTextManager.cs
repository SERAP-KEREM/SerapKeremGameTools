using UnityEngine;
using SerapKeremGameTools._Game._objectPool;
using System.Collections;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Manages the pop-up text animations and pooling.
    /// Handles the creation, animation, and return of pop-up text objects.
    /// </summary>
    public class PopUpTextManager : MonoBehaviour
    {
        [Header("Pop-Up Settings")]
        [SerializeField, Tooltip("The prefab for the pop-up text.")]
        private PopUpText popUpTextPrefab;

        [SerializeField, Tooltip("The parent transform for positioning the pop-up texts.")]
        private Transform parent;

        [SerializeField, Tooltip("The initial pool size for pop-up texts.")]
        private int poolSize = 10;

        [SerializeField, Tooltip("The delay before the pop-up text disappears.")]
        private float hideDelay = 1f;

        [Header("Animation Settings")]
        [SerializeField, Tooltip("Duration of the animation.")]
        private float animationDuration = 0.5f;

        [SerializeField, Tooltip("The height the pop-up text will bounce.")]
        private float bounceHeight = 2f;

        [SerializeField, Tooltip("The number of times the pop-up text will bounce.")]
        private int bounceCount = 3;

        [SerializeField, Tooltip("The offset used for the sliding animation.")]
        private Vector3 slideOffset = new Vector3(0, 2, 0);

        private ObjectPool<PopUpText> popUpTextPool;

        /// <summary>
        /// The singleton instance of PopUpTextManager.
        /// </summary>
        public static PopUpTextManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            popUpTextPool = new ObjectPool<PopUpText>(popUpTextPrefab, poolSize, parent);
        }

        /// <summary>
        /// Shows a pop-up text at a given position with a specified animation type.
        /// </summary>
        /// <param name="position">The position where the pop-up text will appear.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="customDuration">The custom duration for the text to stay on screen.</param>
        /// <param name="animationType">The animation type for the pop-up text.</param>
        public void ShowPopUpText(Vector3 position, string text, float customDuration, PopUpAnimationType animationType)
        {
            PopUpText popUpText = popUpTextPool.GetObject();
            popUpText.transform.position = position;
            popUpText.Initialize(text);

            // Choose animation type
            float duration = customDuration > 0 ? customDuration : animationDuration; // Default or custom duration
            StartCoroutine(HandleAnimation(popUpText, duration, animationType));

            StartCoroutine(ReturnPopUpTextAfterDelay(popUpText, duration + hideDelay));
        }

        /// <summary>
        /// Handles the chosen animation for the pop-up text.
        /// </summary>
        /// <param name="popUpText">The pop-up text to animate.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="animationType">The type of animation to apply.</param>
        /// <returns>Returns an IEnumerator for coroutine.</returns>
        private IEnumerator HandleAnimation(PopUpText popUpText, float duration, PopUpAnimationType animationType)
        {
            switch (animationType)
            {
                case PopUpAnimationType.ScaleAndFade:
                    yield return ScaleAndFadeAnimation(popUpText, duration);
                    break;
                case PopUpAnimationType.SlideUp:
                    yield return SlideAnimation(popUpText, slideOffset, duration);
                    break;
                case PopUpAnimationType.SlideDown:
                    yield return SlideAnimation(popUpText, -slideOffset, duration);
                    break;
                case PopUpAnimationType.Bounce:
                    yield return BounceAnimation(popUpText, duration);
                    break;
            }
        }

        /// <summary>
        /// Animates the pop-up text with scaling and fading effects.
        /// </summary>
        /// <param name="popUpText">The pop-up text to animate.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <returns>Returns an IEnumerator for coroutine.</returns>
        private IEnumerator ScaleAndFadeAnimation(PopUpText popUpText, float duration)
        {
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                popUpText.transform.localScale = Vector3.Lerp(startScale, endScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(duration);

            elapsedTime = 0f;
            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                popUpText.transform.localScale = Vector3.Lerp(endScale, startScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Animates the pop-up text with sliding motion.
        /// </summary>
        /// <param name="popUpText">The pop-up text to animate.</param>
        /// <param name="offset">The offset direction for the sliding motion.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <returns>Returns an IEnumerator for coroutine.</returns>
        private IEnumerator SlideAnimation(PopUpText popUpText, Vector3 offset, float duration)
        {
            Vector3 startPosition = popUpText.transform.position;
            Vector3 targetPosition = startPosition + offset;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = Mathf.SmoothStep(0, 1, t); // Smoother movement
                popUpText.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            popUpText.transform.position = targetPosition; // Set final position
        }

        /// <summary>
        /// Animates the pop-up text with a bounce effect.
        /// </summary>
        /// <param name="popUpText">The pop-up text to animate.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <returns>Returns an IEnumerator for coroutine.</returns>
        private IEnumerator BounceAnimation(PopUpText popUpText, float duration)
        {
            Vector3 startPosition = popUpText.transform.position;
            float elapsedTime = 0f;

            for (int i = 0; i < bounceCount; i++)
            {
                float t = 0f;
                while (t < 1f)
                {
                    float yOffset = Mathf.Sin(t * Mathf.PI) * bounceHeight;
                    popUpText.transform.position = startPosition + new Vector3(0, yOffset, 0);
                    t += Time.deltaTime / (duration / bounceCount);
                    yield return null;
                }
            }

            popUpText.transform.position = startPosition; // Reset to start position
        }

        /// <summary>
        /// Returns the pop-up text to the object pool after a delay.
        /// </summary>
        /// <param name="popUpText">The pop-up text to return.</param>
        /// <param name="delay">The delay before returning the pop-up text.</param>
        /// <returns>Returns an IEnumerator for coroutine.</returns>
        private IEnumerator ReturnPopUpTextAfterDelay(PopUpText popUpText, float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnPopUpText(popUpText);
        }

        /// <summary>
        /// Returns the pop-up text to the object pool.
        /// </summary>
        /// <param name="popUpText">The pop-up text to return to the pool.</param>
        public void ReturnPopUpText(PopUpText popUpText)
        {
            popUpTextPool.ReturnObject(popUpText);
        }
    }
}
