using SerapKeremGameTools._Game._FPSPlayerSystem;
using UnityEngine;

namespace SerapKeremGameTools._Game._InputSystem
{
    public class PlayerFPSInput : MonoBehaviour
    {
        // Statik özellikler, di?er s?n?flar?n bu verilere eri?ebilmesi için
        public static Vector2 MovementInput { get; private set; }
        public static bool IsSprinting { get; private set; }
        public static bool IsShouldJump { get; private set; }
        public static bool IsShouldCrouch { get; private set; }
        public static bool IsZooming { get; private set; }

        public static float MouseLookInputX { get; private set; }
        public static float MouseLookInputY { get; private set; }

        [Header("Controls")]
        [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
        [SerializeField] public static KeyCode zoomKey = KeyCode.Mouse1;
        [SerializeField] public static KeyCode interactKey = KeyCode.E;

      

        [Header("Movement")]
        [SerializeField] private float mouseSensitivityX = 2.0f;
        [SerializeField] private float mouseSensitivityY = 2.0f;

        private void Update()
        {
            HandleMovementInput();
            HandleActions();
            HandleMouseLook();

            if (Input.GetKey(interactKey))
            {
                FPSInteractionSystem.Instance.TryInteract();
            }
        }

        private void HandleMovementInput()
        {
            MovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        private void HandleActions()
        {
            IsSprinting = Input.GetKey(sprintKey);
            IsShouldJump = Input.GetKeyDown(jumpKey);
            IsShouldCrouch = Input.GetKeyDown(crouchKey);
            IsZooming = Input.GetKey(zoomKey);
        }

        private void HandleMouseLook()
        {
            MouseLookInputX = Input.GetAxis("Mouse X") * mouseSensitivityX;
            MouseLookInputY = Input.GetAxis("Mouse Y") * mouseSensitivityY;
        }
    }
}
