using UnityEngine;
using System.Collections.Generic;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// This class is responsible for testing the pop-up text system.
    /// It listens for user input (mouse click or screen touch) and shows a pop-up with a random message
    /// from a list of options. The pop-up's position, duration, and animation type can be configured.
    /// </summary>
    public class PopupTextTest : MonoBehaviour
    {
        [Header("Pop-Up Settings")]

        [SerializeField, Tooltip("Reference to the PopUpTextManager that handles the pop-up creation.")]
        private PopUpTextManager popUpTextManager;

        [SerializeField, Tooltip("List of messages to choose from when creating a pop-up.")]
        private List<string> popUpTextOptions = new List<string>();

        [SerializeField, Tooltip("The position at which the pop-up text will appear.")]
        private Vector3 popUpPosition = Vector3.zero;

        [SerializeField, Range(0.1f, 2f), Tooltip("Duration for which the pop-up text will remain visible.")]
        private float popUpDuration = 0.5f;

        [SerializeField, Tooltip("Animation type for the pop-up text.")]
        private PopUpAnimationType animationType = PopUpAnimationType.ScaleAndFade;

        private System.Random random = new System.Random();

        /// <summary>
        /// Update is called once per frame. This checks for user input and triggers the pop-up if needed.
        /// </summary>
        private void Update()
        {
            // Check for mouse click or screen touch to show the pop-up
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                ShowPopUp();
            }
        }

        /// <summary>
        /// This method selects a random message from the list of options and shows it as a pop-up.
        /// </summary>
        private void ShowPopUp()
        {
            if (popUpTextOptions.Count == 0) return;

            // Randomly choose a message from the available options
            string message = popUpTextOptions[random.Next(popUpTextOptions.Count)];

            // Show the pop-up with the selected message, duration, and animation type
            popUpTextManager.ShowPopUpText(popUpPosition, message, popUpDuration, animationType);
        }
    }
}
