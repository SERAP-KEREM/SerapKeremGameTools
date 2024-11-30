using System.Collections.Generic;
using UnityEngine;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Test script for displaying pop-up icon (sprite) messages randomly.
    /// </summary>
    public class PopupIconTest : MonoBehaviour
    {
        [Header("Pop-Up Sprite Settings")]
        [SerializeField, Tooltip("List of sprites to choose from when creating a pop-up.")]
        private List<Sprite> popUpSpriteOptions = new List<Sprite>();

        [SerializeField, Tooltip("The position where the pop-up will appear.")]
        private Vector3 popUpPosition = Vector3.zero;

        [SerializeField, Tooltip("The duration the pop-up stays on screen.")]
        private float popUpDuration = 2f;

        [SerializeField, Tooltip("The type of animation for the pop-up.")]
        private PopUpAnimationType animationType = PopUpAnimationType.ScaleAndFade;

        private System.Random random = new System.Random();

        void Start()
        {
            // Test by showing a pop-up every 2 seconds
            InvokeRepeating("ShowRandomPopUp", 0f, 2f);
        }

        void ShowRandomPopUp()
        {
            if (popUpSpriteOptions.Count == 0) return;

            Debug.Log("ShowPopUp");

            // Randomly choose a sprite from the available options
            Sprite sprite = popUpSpriteOptions[random.Next(popUpSpriteOptions.Count)];

            // Show the pop-up with the selected sprite, duration, and animation type
            PopUpIconManager.Instance.ShowPopUpIcon(popUpPosition, sprite, popUpDuration, animationType);
        }
    }
}
