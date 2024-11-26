using System.Collections.Generic;
using UnityEngine;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Test script for displaying pop-up text messages randomly.
    /// </summary>
    public class PopupTextTest : MonoBehaviour
    {
        [Header("Pop-Up Settings")]
        [SerializeField, Tooltip("List of messages to choose from when creating a pop-up.")]
        private List<string> _popUpTextOptions = new List<string>();

        [SerializeField, Tooltip("The position where the pop-up will appear.")]
        private Vector3 _popUpPosition = Vector3.zero;

        [SerializeField, Tooltip("The duration the pop-up stays on screen.")]
        private float _popUpDuration = 2f;

        [SerializeField, Tooltip("The type of animation for the pop-up.")]
        private PopUpAnimationType _animationType = PopUpAnimationType.ScaleAndFade;

        private System.Random _random = new System.Random();

        void Start()
        {
            // Test by showing a pop-up every 2 seconds
            InvokeRepeating("ShowRandomPopUp", 0f, 2f);
        }

        void ShowRandomPopUp()
        {
            if (_popUpTextOptions.Count == 0) return;

            Debug.Log("ShowPopUp");

            // Randomly choose a message from the available options
            string message = _popUpTextOptions[_random.Next(_popUpTextOptions.Count)];

            // Show the pop-up with the selected message, duration, and animation type
            PopUpTextManager.Instance.ShowPopUpText(_popUpPosition, message, _popUpDuration, _animationType);

        }
    }
}
