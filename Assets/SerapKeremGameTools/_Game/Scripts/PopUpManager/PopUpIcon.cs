using UnityEngine;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PopUpIcon : MonoBehaviour
    {
        [Tooltip("The SpriteRenderer component used to display the pop-up icon.")]
        private SpriteRenderer spriteRenderer;

        [Tooltip("The initial scale of the pop-up icon.")]
        private Vector3 initialScale;

        private void Awake()
        {
            // Get the SpriteRenderer component and store the initial scale
            spriteRenderer = GetComponent<SpriteRenderer>();
            initialScale = transform.localScale;
        }

        /// <summary>
        /// Initializes the pop-up icon with a specific sprite and scale multiplier.
        /// </summary>
        /// <param name="sprite">The sprite to display in the pop-up icon.</param>
        /// <param name="scaleMultiplier">Multiplier for the scale of the pop-up icon.</param>
        public void Initialize(Sprite sprite, float scaleMultiplier = 1f)
        {
            if (sprite == null)
            {
                Debug.LogError("PopUpSpriteRenderer: The sprite to initialize is null!");
                return;
            }

            spriteRenderer.sprite = sprite; // Set the sprite
            transform.localScale = initialScale * scaleMultiplier; // Adjust the scale
            spriteRenderer.color = Color.white; // Reset color to default
        }

        /// <summary>
        /// Resets the properties of the pop-up icon to their default state.
        /// </summary>
        public void ResetProperties()
        {
            transform.localScale = initialScale; // Reset scale
            spriteRenderer.color = Color.white; // Reset color to default
            spriteRenderer.sprite = null; // Clear the sprite
        }
    }
}
