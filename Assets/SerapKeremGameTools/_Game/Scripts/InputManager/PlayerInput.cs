using SerapKeremGameTools._Game._Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace SerapKeremGameTools._Game._InputSystem
{
    /// <summary>
    /// Handles player input, including mouse position tracking and mouse events.
    /// </summary>
    public class PlayerInput : MonoSingleton<PlayerInput>
    {
        /// <summary>
        /// Current mouse position in world coordinates.
        /// </summary>
        [Tooltip("Current mouse position in world coordinates.")]
        public Vector3 MousePosition { get; private set; }

        /// <summary>
        /// Event invoked when the left mouse button is pressed down.
        /// </summary>
        [Tooltip("Event triggered when the left mouse button is pressed down.")]
        public UnityEvent OnMouseDownEvent = new UnityEvent();

        /// <summary>
        /// Event invoked while the left mouse button is held down.
        /// </summary>
        [Tooltip("Event triggered while the left mouse button is held down.")]
        public UnityEvent OnMouseHeldEvent = new UnityEvent();

        /// <summary>
        /// Event invoked when the left mouse button is released.
        /// </summary>
        [Tooltip("Event triggered when the left mouse button is released.")]
        public UnityEvent OnMouseUpEvent = new UnityEvent();

        [Tooltip("Reference to the main camera in the scene.")]
        private Camera mainCamera;

        /// <summary>
        /// Initializes the PlayerInput singleton and assigns the main camera.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
#if UNITY_EDITOR
                Debug.LogError("No Main Camera found in the scene!");
#endif
            }
        }

        /// <summary>
        /// Updates mouse position and processes mouse input events.
        /// </summary>
        private void Update()
        {
            if (mainCamera != null)
            {
                // Convert mouse position to world coordinates
                MousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
            }

            // Mouse input events
            if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
            {
                OnMouseDownEvent.Invoke();
            }

            if (Input.GetMouseButton(0)) // Left mouse button held
            {
                OnMouseHeldEvent.Invoke();
            }

            if (Input.GetMouseButtonUp(0)) // Left mouse button released
            {
                OnMouseUpEvent.Invoke();
            }
        }
    }
}
