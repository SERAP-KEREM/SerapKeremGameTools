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

        private void Update()
        {
            // Fare pozisyonunu dünya koordinatlar?na dönü?tür
            MousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

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
