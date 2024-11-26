using UnityEngine;
using SerapKeremGameTools._Game._Singleton;
using System.Collections;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Abstract base class for managing pop-up objects in the scene.
    /// Provides core functionality for handling animations, pooling, and object lifecycle.
    /// Inherits from MonoSingleton to ensure a single instance per type.
    /// </summary>
    /// <typeparam name="T">The type of the pop-up object (e.g., text, icon).</typeparam>
    public abstract class PopUpManager<T, TManager> : MonoSingleton<TManager>
        where T : MonoBehaviour
        where TManager : PopUpManager<T, TManager>
    {
        [Header("Pool Settings")]
        [SerializeField, Tooltip("The initial pool size for pop-up objects.")]
        protected int poolSize = 10;

        [Header("Animation Settings")]
        [SerializeField, Tooltip("Duration of the animation.")]
        protected float animationDuration = 0.5f;

        [SerializeField, Tooltip("Delay before the pop-up disappears.")]
        protected float hideDelay = 1f;

        [SerializeField, Tooltip("Height for bounce animations.")]
        protected float bounceHeight = 2f;

        [SerializeField, Tooltip("Number of bounces for bounce animations.")]
        protected int bounceCount = 3;

        [SerializeField, Tooltip("Offset for slide animations.")]
        protected Vector3 slideOffset = new Vector3(0, 2, 0);

        /// <summary>
        /// Called during initialization. Can be extended by derived classes.
        /// </summary>
        protected virtual void Awake()
        {
#if UNITY_EDITOR
            Debug.Log($"[{typeof(TManager)}] Awake initialized.");
#endif
            // Additional initialization logic can go here.
        }

        /// <summary>
        /// Handles animation logic specific to the pop-up object type.
        /// </summary>
        /// <param name="popUpObject">The pop-up object to animate.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="animationType">The type of animation to apply.</param>
        /// <returns>An IEnumerator for coroutine execution.</returns>
        protected abstract IEnumerator HandleAnimation(T popUpObject, float duration, PopUpAnimationType animationType);

        /// <summary>
        /// Returns the pop-up object to its pool after its use.
        /// </summary>
        /// <param name="popUpObject">The pop-up object to return.</param>
        /// <param name="delay">The delay before returning the object to the pool.</param>
        /// <returns>An IEnumerator for coroutine execution.</returns>
        protected abstract IEnumerator ReturnPopUpObjectAfterDelay(T popUpObject, float delay);
    }
}
