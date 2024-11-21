using UnityEngine;
using SerapKeremGameTools._Game._objectPool;
using System.Collections;
using SerapKeremGameTools._Game._Singleton;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    public class PopUpSpriteRendererManager : MonoSingleton<PopUpSpriteRendererManager>
    {
        [Header("Pop-Up Settings")]
        [SerializeField, Tooltip("The prefab for the pop-up sprite (using SpriteRenderer).")]
        private PopUpSpriteRenderer popUpSpritePrefab;

        [SerializeField, Tooltip("The initial pool size for pop-up sprites.")]
        private int poolSize = 10;

        [SerializeField, Tooltip("The delay before the pop-up sprite disappears.")]
        private float hideDelay = 1f;

        [Header("Animation Settings")]
        [SerializeField, Tooltip("Duration of the animation.")]
        private float animationDuration = 0.5f;

        [SerializeField, Tooltip("The height the pop-up sprite will bounce.")]
        private float bounceHeight = 2f;

        [SerializeField, Tooltip("The number of times the pop-up sprite will bounce.")]
        private int bounceCount = 3;

        [SerializeField, Tooltip("The offset used for the sliding animation.")]
        private Vector3 slideOffset = new Vector3(0, 2, 0);

        private ObjectPool<PopUpSpriteRenderer> popUpSpritePool;

        protected override void Awake()
        {
            base.Awake();
            // Create Object Pool for PopUpSpriteRenderer
            popUpSpritePool = new ObjectPool<PopUpSpriteRenderer>(popUpSpritePrefab, poolSize, transform);
        }

        public void ShowPopUpSprite(Vector3 position, Sprite sprite, float customDuration, PopUpAnimationType animationType)
        {
            // Get a PopUpSpriteRenderer from the pool
            PopUpSpriteRenderer popUpSprite = popUpSpritePool.GetObject();

            // Set the position of the sprite
            popUpSprite.transform.position = position;
            // Initialize the sprite's visual properties
            popUpSprite.Initialize(sprite);

            float duration = customDuration > 0 ? customDuration : animationDuration; // Default or custom duration

            // Start animation coroutine
            StartCoroutine(HandleAnimation(popUpSprite, duration, animationType));
            // After the delay, return the object to the pool
            StartCoroutine(ReturnPopUpSpriteAfterDelay(popUpSprite, duration + hideDelay));
        }

        private IEnumerator HandleAnimation(PopUpSpriteRenderer popUpSprite, float duration, PopUpAnimationType animationType)
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

        private IEnumerator ScaleAndFadeAnimation(PopUpSpriteRenderer popUpSprite, float duration)
        {
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                popUpSprite.transform.localScale = Vector3.Lerp(startScale, endScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

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

        private IEnumerator SlideAnimation(PopUpSpriteRenderer popUpSprite, Vector3 offset, float duration)
        {
            Vector3 startPosition = popUpSprite.transform.position;
            Vector3 targetPosition = startPosition + offset;
            float elapsedTime = 0f;

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

        private IEnumerator BounceAnimation(PopUpSpriteRenderer popUpSprite, float duration)
        {
            Vector3 startPosition = popUpSprite.transform.position;
            float elapsedTime = 0f;

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

        private IEnumerator ReturnPopUpSpriteAfterDelay(PopUpSpriteRenderer popUpSprite, float delay)
        {
            yield return new WaitForSeconds(delay);
            // Return the sprite to the pool after animation is done
            ReturnPopUpSprite(popUpSprite);
        }

        public void ReturnPopUpSprite(PopUpSpriteRenderer popUpSprite)
        {
            // Reset the sprite properties if needed before returning
            popUpSprite.ResetProperties();
            // Return the sprite to the pool
            popUpSpritePool.ReturnObject(popUpSprite);
        }

        public PopUpSpriteRenderer GetPopUpSpriteRenderer()
        {
            // Object Pool'dan bir PopUpSpriteRenderer nesnesi al?r
            return popUpSpritePool.GetObject();
        }

    }
}
