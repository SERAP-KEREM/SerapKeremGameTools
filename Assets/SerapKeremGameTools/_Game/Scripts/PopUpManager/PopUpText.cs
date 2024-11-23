using UnityEngine;
using TMPro;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Manages the initialization and reset of pop-up text in 3D space.
    /// </summary>
    public class PopUpText : MonoBehaviour
    {
        [Tooltip("The TextMeshPro component used to display the pop-up text.")]
        [SerializeField] private TextMeshPro textComponent;

        private void Awake()
        {
            if (textComponent == null)
            {
                textComponent = GetComponent<TextMeshPro>();
                if (textComponent == null)
                {
                    Debug.LogError("TextMeshPro component is missing on the GameObject!", this);
                }
            }
        }

        /// <summary>
        /// Initializes the pop-up text with the provided content.
        /// </summary>
        /// <param name="text">The string to display in the pop-up.</param>
        public void Initialize(string text)
        {
            if (textComponent != null)
            {
                textComponent.text = text;
            }
            else
            {
                Debug.LogError("TextMeshPro component is not assigned in the PopUpText script.");
            }
        }

        /// <summary>
        /// Resets the properties of the pop-up text, such as clearing the text and resetting the transform.
        /// </summary>
        public void ResetProperties()
        {
            if (textComponent != null)
            {
                textComponent.text = string.Empty; // Clears the pop-up text
            }
            else
            {
                Debug.LogWarning("TextMeshPro component is missing when attempting to reset properties.", this);
            }

            // Resets position and scale to their default values
            transform.position = Vector3.zero;
            transform.localScale = Vector3.one;
        }
    }
}
