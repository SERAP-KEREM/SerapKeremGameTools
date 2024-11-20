using UnityEngine;
using System.Collections.Generic;
using SerapKeremGameTools._Game._PopUpSystem; // PopUpSpriteRendererManager'? kullanabilmek için

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// This class is responsible for testing a SpriteRenderer-based pop-up system.
    /// It listens for user input (mouse click or screen touch) and shows a pop-up with a random sprite
    /// from a list of options. The pop-up's position and duration can be configured.
    /// </summary>
    public class SpritePopupTest : MonoBehaviour
    {
        [Header("Pop-Up Settings")]

        [SerializeField, Tooltip("List of sprites to choose from when creating a pop-up.")]
        private List<Sprite> _popUpSpriteOptions = new List<Sprite>();

        [SerializeField, Tooltip("The position at which the pop-up sprite will appear.")]
        private Vector3 _popUpPosition = Vector3.zero;

        [SerializeField, Range(0.1f, 2f), Tooltip("Duration for which the pop-up sprite will remain visible.")]
        private float _popUpDuration = 0.5f;

        private System.Random _random = new System.Random();

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
        /// This method selects a random sprite from the list of options and shows it as a pop-up.
        /// </summary>
        private void ShowPopUp()
        {
            if (_popUpSpriteOptions.Count == 0) return;

            // Randomly choose a sprite from the available options
            Sprite selectedSprite = _popUpSpriteOptions[_random.Next(_popUpSpriteOptions.Count)];

            // Get a PopUpSpriteRenderer from the Object Pool
            PopUpSpriteRenderer popUpSprite = PopUpSpriteRendererManager.Instance.GetPopUpSpriteRenderer();

            if (popUpSprite)
            {
                // Set the position and sprite
                popUpSprite.transform.position = _popUpPosition;
                popUpSprite.Initialize(selectedSprite);

                // Show the sprite with animation
                PopUpSpriteRendererManager.Instance.ShowPopUpSprite(_popUpPosition, selectedSprite, _popUpDuration, PopUpAnimationType.ScaleAndFade);
            }
        }
    }
}
