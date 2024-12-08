using SerapKeremGameTools._Game._InputSystem;
using SerapKeremGameTools._Game._Singleton;
using UnityEngine;
namespace SerapKeremGameTools._Game._FPSPlayerSystem
{
    public class FPSInteractionSystem : MonoSingleton<FPSInteractionSystem>
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private LayerMask interactionLayer;
        private Interactable currentInteractable;
        private Camera playerCamera;

        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            playerCamera = Camera.main;
        }

        private void Update()
        {
            HandleFocus();
            if (Input.GetKeyDown(PlayerFPSInput.interactKey))
            {
                TryInteract();
            }
        }

        private void HandleFocus()
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance, interactionLayer))
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();
                if (interactableObject != null && interactableObject != currentInteractable)
                {
                    currentInteractable?.OnLoseFocus();
                    currentInteractable = interactableObject;
                    currentInteractable.OnFocus();
                }
            }
            else if (currentInteractable != null)
            {
                currentInteractable.OnLoseFocus();
                currentInteractable = null;
            }
        }

        public void TryInteract()
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnInteract();
            }
        }
    }
}
