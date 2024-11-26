using UnityEngine;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Manages a pop-up displaying an icon.
    /// </summary>
    public class PopUpIcon : PopUp
    {
        [Header("Icon Settings")]
        [Tooltip("The SpriteRenderer component used to display the pop-up icon.")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        protected override void Awake()
        {
            base.Awake();

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();

                if (spriteRenderer == null)
                {
                    Debug.LogWarning("PopUpIcon: SpriteRenderer component is missing. Assign it in the inspector or attach it to the GameObject.", this);
                }
            }
        }

        /// <summary>
        /// Initializes the pop-up with the specified icon and optional scale.
        /// </summary>
        /// <param name="args">Expected: Sprite as the first argument, and optionally a float for scale multiplier.</param>
        public override void Initialize(params object[] args)
        {
            if (!ValidateArguments(args, out Sprite sprite, out float? optionalScale))
            {
                Debug.LogError("PopUpIcon: Initialization failed due to invalid arguments. Expected: Sprite and optional float.", this);
                return;
            }

            spriteRenderer.sprite = sprite;

            if (optionalScale.HasValue)
            {
                transform.localScale = initialScale * optionalScale.Value;
            }

            StartCoroutine(PlayScaleAnimation());
        }

        /// <summary>
        /// Resets the pop-up icon and its properties.
        /// </summary>
        public override void ResetProperties()
        {
            base.ResetProperties();

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = null;
            }
        }

        /// <summary>
        /// Validates and extracts arguments for initializing the pop-up.
        /// </summary>
        /// <param name="args">The arguments passed to the Initialize method.</param>
        /// <param name="sprite">Extracted Sprite argument.</param>
        /// <param name="scale">Optional float scale multiplier argument.</param>
        /// <returns>True if arguments are valid, false otherwise.</returns>
        private bool ValidateArguments(object[] args, out Sprite sprite, out float? scale)
        {
            sprite = null;
            scale = null;

            if (args == null || args.Length < 1 || !(args[0] is Sprite))
            {
                return false;
            }

            sprite = (Sprite)args[0];

            if (args.Length > 1 && args[1] is float floatArg)
            {
                scale = floatArg;
            }

            return true;
        }
    }
}
