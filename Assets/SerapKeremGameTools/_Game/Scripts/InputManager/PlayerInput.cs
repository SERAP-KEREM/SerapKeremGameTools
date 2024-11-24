using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SerapKeremGameTools._Game._InputSystem
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector3 MousePosition { get; private set; }
        public UnityEvent OnMouseDownEvent = new UnityEvent();
        public UnityEvent OnMouseHeldEvent = new UnityEvent();
        public UnityEvent OnMouseUpEvent = new UnityEvent();

        private Camera mainCamera;

        private void Awake()
        {
            // Main Camera referans?n? kod içinde al?yoruz
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("No Main Camera found in the scene!");
            }
        }

        private void Update()
        {
            if (mainCamera != null)
            {
                // Fare pozisyonunu dünya koordinatlar?na dönü?tür
                MousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
            }

            // Fare t?klama olaylar?
            if (Input.GetMouseButtonDown(0)) // Sol fare tu?u
            {
                OnMouseDownEvent.Invoke();
            }

            if (Input.GetMouseButton(0)) // Fare bas?l? tutuldu?unda
            {
                OnMouseHeldEvent.Invoke();
            }

            if (Input.GetMouseButtonUp(0)) // Fare b?rak?ld???nda
            {
                OnMouseUpEvent.Invoke();
            }
        }
    }
}
