using SerapKeremGameTools._Game._InputSystem;
using UnityEngine;

namespace SerapKeremGameTools.InputSystem
{
    public class TestSelectAndCollect : MonoBehaviour, ISelectable, ICollectable
    {
        private Renderer objectRenderer;
        private Color originalColor;

        private void Awake()
        {
            objectRenderer = GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                originalColor = objectRenderer.material.color;
            }
        }

        public void Select()
        {
            Debug.Log($"{gameObject.name} has been selected!");
            if (objectRenderer != null)
            {
                objectRenderer.material.color = Color.green; // Seçildi?inde ye?il renge boyan?r
            }
        }

        public void DeSelect()
        {
            Debug.Log($"{gameObject.name} has been deselected!");
            if (objectRenderer != null)
            {
                objectRenderer.material.color = originalColor; // Orijinal renge döner
            }
        }

        public void Collect()
        {
            Debug.Log($"{gameObject.name} has been collected!");
            Destroy(gameObject, 0.5f); // Toplama i?leminden sonra nesneyi yok et
        }
    }
}
