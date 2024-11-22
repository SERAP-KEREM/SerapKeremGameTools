using UnityEngine;
using SerapKeremGameTools._Game._objectPool;
using System.Collections;
using SerapKeremGameTools._Game._Singleton;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    public class PopUpTextManager : MonoSingleton<PopUpTextManager>
    {
        [Header("Pop-Up Settings")]
        [SerializeField, Tooltip("The prefab for the pop-up text (using TextMeshPro or 3D Text).")]
        private PopUpText popUpTextPrefab;

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

        [Header("Font Settings")]
        [SerializeField, Tooltip("Scale applied to the pop-up text during the animation.")]
        private Vector3 _popUpTextScale = Vector3.one;  // Global scale for pop-up text

        private ObjectPool<PopUpText> popUpTextPool;

        protected override void Awake()
        {
            base.Awake();
            popUpTextPool = new ObjectPool<PopUpText>(popUpTextPrefab, poolSize, transform);
        }

        public  void ShowPopUpText(Vector3 position, string text, float customDuration, PopUpAnimationType animationType)
        {
            PopUpText popUpText = popUpTextPool.GetObject();
            popUpText.transform.position = position;
            popUpText.Initialize(text);

            // Apply the pop-up scale
            popUpText.transform.localScale = _popUpTextScale;

            float duration = customDuration > 0 ? customDuration : animationDuration; // Default or custom duration
            StartCoroutine(HandleAnimation(popUpText, duration, animationType));
            StartCoroutine(ReturnPopUpTextAfterDelay(popUpText, duration + hideDelay));
        }

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

        private IEnumerator ScaleAndFadeAnimation(PopUpText popUpText, float duration)
        {
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = _popUpTextScale;  // Use the scale defined in inspector
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

            popUpText.transform.position = targetPosition;
        }

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

            popUpText.transform.position = startPosition;
        }

        private IEnumerator ReturnPopUpTextAfterDelay(PopUpText popUpText, float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnPopUpText(popUpText);
        }

        public void ReturnPopUpText(PopUpText popUpText)
        {
            popUpTextPool.ReturnObject(popUpText);
        }
    }
}
