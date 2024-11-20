using UnityEngine;
using TMPro;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Manages the initialization of pop-up text in 3D space.
    /// </summary>
    public class PopUpText : MonoBehaviour
    {
        [Tooltip("The TextMeshPro component used to display the text.")]
        [SerializeField] private TextMeshPro textComponent;

        private void Awake()
        {
            // E?er Inspector'da atanmad?ysa otomatik olarak atay?n.
            if (textComponent == null)
            {
                textComponent = GetComponent<TextMeshPro>();
                if (textComponent == null)
                {
                    Debug.LogError("Text component is missing on the GameObject!", this);
                }
            }
        }

        /// <summary>
        /// Initializes the pop-up text with the given content.
        /// </summary>
        /// <param name="text">The text to display.</param>
        public void Initialize(string text)
        {
            if (textComponent != null)
            {
                textComponent.text = text;
            }
            else
            {
                Debug.LogError("Text component is not assigned in the PopUpText.");
            }
        }
    }
}
