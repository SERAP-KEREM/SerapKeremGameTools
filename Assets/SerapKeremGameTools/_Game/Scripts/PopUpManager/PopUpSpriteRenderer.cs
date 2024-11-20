using UnityEngine;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    public class PopUpSpriteRenderer : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        // Animation properties
        private Vector3 initialScale;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            initialScale = transform.localScale;
        }

        public void Initialize(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            // Set the initial scale if needed
            transform.localScale = initialScale;
            // Reset any other properties that are important, such as alpha
            spriteRenderer.color = Color.white; // Reset color
        }

        // Reset the sprite's properties when returning to the pool
        public void ResetProperties()
        {
            transform.localScale = initialScale; // Reset scale
            spriteRenderer.color = Color.white;  // Reset color (or any other properties you need)
            spriteRenderer.sprite = null; // Clear the sprite
        }
    }
}
