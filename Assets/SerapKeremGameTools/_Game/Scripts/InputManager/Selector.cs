using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace SerapKeremGameTools._Game._InputSystem
{

    public class Selector : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput; // PlayerInput referansı
        public float raycastLength = 10f;
        private ISelectable selectedObject;

        private void OnEnable()
        {
            // PlayerInput null kontrolü
            if (playerInput == null)
            {
                Debug.LogError("PlayerInput reference is missing!");
                return;
            }

            // Olaylara abone ol
            playerInput.OnMouseDownEvent.AddListener(SelectObject);
            playerInput.OnMouseUpEvent.AddListener(DeselectObject);
        }

        private void OnDisable()
        {
            // Olaylardan çık
            if (playerInput != null)
            {
                playerInput.OnMouseDownEvent.RemoveListener(SelectObject);
                playerInput.OnMouseUpEvent.RemoveListener(DeselectObject);
            }
        }

        private void SelectObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(playerInput.MousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastLength))
            {
                selectedObject = hit.collider.GetComponent<ISelectable>();
                selectedObject?.Select();
            }
        }

        private void DeselectObject()
        {
            if (selectedObject != null)
            {
                selectedObject.DeSelect();

                // Eğer seçilen obje ICollectable ise toplama işlemini tetikle
                if (selectedObject is ICollectable collectable)
                {
                    collectable.Collect();
                }

                // Seçimi sıfırla
                selectedObject = null;
            }
        }
    }
}