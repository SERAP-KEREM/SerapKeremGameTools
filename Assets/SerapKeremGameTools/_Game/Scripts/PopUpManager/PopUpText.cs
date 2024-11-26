using TMPro;
using UnityEngine;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Manages a pop-up displaying text.
    /// </summary>
    public class PopUpText : PopUp
    {
        [Header("Text Settings")]
        [Tooltip("The TextMeshPro component used to display the pop-up text.")]
        [SerializeField] private TextMeshPro textComponent;

        /// <summary>
        /// Called during initialization. Checks if the textComponent is set.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            Debug.Log($"[{nameof(PopUpText)}] Awake initialized.");
#endif
            if (textComponent == null)
            {
                textComponent = GetComponent<TextMeshPro>();
#if UNITY_EDITOR
                Debug.LogWarning("PopUpText: TextMeshPro component is missing, attempting to fetch from the GameObject.", this);
#endif
            }
        }

        /// <summary>
        /// Initializes the pop-up with the specified text.
        /// </summary>
        /// <param name="args">Expected: a single string argument for the text.</param>
        public override void Initialize(params object[] args)
        {
            if (args.Length == 0 || !(args[0] is string text))
            {
                Debug.LogError("PopUpText: Invalid arguments for initialization.", this);
                return;
            }

            textComponent.text = text;
            StartCoroutine(PlayScaleAnimation());
        }

        /// <summary>
        /// Resets the pop-up text and its properties.
        /// </summary>
        public override void ResetProperties()
        {
            base.ResetProperties();
            if (textComponent != null)
            {
                textComponent.text = string.Empty;
            }
        }
    }
}
