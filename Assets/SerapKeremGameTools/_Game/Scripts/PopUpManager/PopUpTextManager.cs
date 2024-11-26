using UnityEngine;
using System.Collections;
using SerapKeremGameTools._Game._objectPool;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Manages the creation, animation, and pooling of text-based pop-ups.
    /// Inherits from PopUpManager for shared behavior.
    /// </summary>
    public class PopUpTextManager : PopUpManager<PopUpText, PopUpTextManager>
    {
        [Header("Pop-Up Text Settings")]
        [SerializeField, Tooltip("Prefab for the pop-up text.")]
        private PopUpText popUpTextPrefab;

        [SerializeField, Tooltip("Scale applied to the pop-up text.")]
        private Vector3 popUpTextScale = Vector3.one;

        private ObjectPool<PopUpText> popUpTextPool;

        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            Debug.Log($"[{nameof(PopUpTextManager)}] Awake initialized.");
#endif
            popUpTextPool=new ObjectPool<PopUpText> (popUpTextPrefab,poolSize,transform);

        }

        /// <summary>
        /// Displays a pop-up text at a specified position with custom animation.
        /// </summary>
        public void ShowPopUpText(Vector3 position, string text, float customDuration, PopUpAnimationType animationType)
        {
#if UNITY_EDITOR
            Debug.Log($"[{nameof(PopUpTextManager)}] Showing pop-up text: {text} at position: {position}.");
#endif
            PopUpText popUpText = popUpTextPool.GetObject();
            popUpText.transform.position = position;
            popUpText.Initialize(text);
            popUpText.transform.localScale = popUpTextScale;

            float duration = customDuration > 0 ? customDuration : animationDuration;

            StartCoroutine(HandleAnimation(popUpText, duration, animationType));
            StartCoroutine(ReturnPopUpObjectAfterDelay(popUpText, duration + hideDelay));
        }

        /// <summary>
        /// Handles animations for the pop-up icon.
        /// </summary>
        protected override IEnumerator HandleAnimation(PopUpText popUpText, float duration, PopUpAnimationType animationType)
        {
#if UNITY_EDITOR
            Debug.Log($"[{nameof(PopUpTextManager)}] Starting animation: {animationType} for duration: {duration}.");
#endif
            switch (animationType)
            {
                case PopUpAnimationType.ScaleAndFade:
                    yield return ScaleAndFadeAnimation(popUpText, duration);
                    break;
                case PopUpAnimationType.SlideUp:
                    yield return SlideAnimation(popUpText, slideOffset, duration);
                    break;
                case PopUpAnimationType.Bounce:
                    yield return BounceAnimation(popUpText, duration);
                    break;
                default:
                    yield return ScaleAndFadeAnimation(popUpText, duration);
                    break;
            }
        }

        /// <summary>
        /// Animates scaling and fading of the pop-up icon.
        /// </summary>
        private IEnumerator ScaleAndFadeAnimation(PopUpText popUpText, float duration)
        {
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                float progress = elapsedTime / duration;
                popUpText.transform.localScale = Vector3.Lerp(startScale, endScale, progress);
                yield return null;
                elapsedTime += Time.deltaTime;
            }
            popUpText.transform.localScale = endScale;
        }

        /// <summary>
        /// Animates sliding of the pop-up icon.
        /// </summary>
        private IEnumerator SlideAnimation(PopUpText popUpText, Vector3 offset, float duration)
        {
#if UNITY_EDITOR
            Debug.Log($"[{nameof(PopUpTextManager)}] Starting slide animation with offset: {offset}.");
#endif
            Vector3 startPosition = popUpText.transform.position;
            Vector3 targetPosition = startPosition + offset;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float progress = elapsedTime / duration;
                popUpText.transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
                yield return null;
                elapsedTime += Time.deltaTime;
            }

            popUpText.transform.position = targetPosition;
        }

        /// <summary>
        /// Animates bouncing of the pop-up icon.
        /// </summary>
        private IEnumerator BounceAnimation(PopUpText popUpText, float duration)
        {
#if UNITY_EDITOR
            Debug.Log($"[{nameof(PopUpTextManager)}] Starting bounce animation.");
#endif
            Vector3 startPosition = popUpText.transform.position;
            float bounceHeight = 0.5f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float progress = elapsedTime / duration;
                float bounceOffset = Mathf.Sin(progress * Mathf.PI) * bounceHeight;
                popUpText.transform.position = startPosition + new Vector3(0, bounceOffset, 0);
                yield return null;
                elapsedTime += Time.deltaTime;
            }

            popUpText.transform.position = startPosition;
        }

        /// <summary>
        /// Returns the pop-up text object to the pool after a delay.
        /// </summary>
        protected override IEnumerator ReturnPopUpObjectAfterDelay(PopUpText popUpText, float delay)
        {
#if UNITY_EDITOR
            Debug.Log($"[{nameof(PopUpTextManager)}] Returning pop-up object after delay: {delay}.");
#endif
            yield return new WaitForSeconds(delay);
            popUpTextPool.ReturnObject(popUpText);
        }
    }
}
