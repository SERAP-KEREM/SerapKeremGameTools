using UnityEngine;
using TMPro;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// This class represents a pop-up text object that can be initialized with a custom string.
    /// It handles the update of the text content displayed by the TextMeshPro component.
    /// </summary>
    public class PopUpText : MonoBehaviour
    {
        [SerializeField, Tooltip("The TextMeshProUGUI component that displays the pop-up text.")]
        private TextMeshProUGUI textComponent;

        /// <summary>
        /// Initializes the pop-up text with a given string.
        /// This method updates the text displayed in the TextMeshProUGUI component.
        /// </summary>
        /// <param name="text">The text to display in the pop-up.</param>
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
