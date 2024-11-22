using UnityEngine;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PopUpIcon : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        // Animation properties
        private Vector3 initialScale;

        private void Awake()
        {
            // SpriteRenderer bile?enini al ve ba?lang?ç ölçe?ini kaydet
            spriteRenderer = GetComponent<SpriteRenderer>();
            initialScale = transform.localScale;
        }

        /// <summary>
        /// Pop-up Sprite'? ba?lat?r.
        /// </summary>
        /// <param name="sprite">Gösterilecek Sprite.</param>
        /// <param name="scaleMultiplier">Sprite'?n büyüklü?ünü belirleyen çarpan.</param>
        public void Initialize(Sprite sprite, float scaleMultiplier = 1f)
        {
            if (sprite == null)
            {
                Debug.LogError("PopUpSpriteRenderer: Initialize edilmek istenen sprite null!");
                return;
            }

            spriteRenderer.sprite = sprite; // Sprite'? ata
            transform.localScale = initialScale * scaleMultiplier; // Ölçe?i çarpan ile ayarla
            spriteRenderer.color = Color.white; // Renk s?f?rla
        }

        /// <summary>
        /// Pop-up Sprite'? varsay?lan durumuna s?f?rlar.
        /// </summary>
        public void ResetProperties()
        {
            transform.localScale = initialScale; // Ölçe?i s?f?rla
            spriteRenderer.color = Color.white; // Renk s?f?rla
            spriteRenderer.sprite = null; // Sprite'? temizle
        }

        /// <summary>
        /// Sprite'?n ölçe?ini dinamik olarak ayarlar.
        /// </summary>
        /// <param name="scaleMultiplier">Ölçek çarpan?.</param>
        public void SetScale(float scaleMultiplier)
        {
            transform.localScale = initialScale * scaleMultiplier;
        }
    }
}
