using UnityEngine;
using System.Collections;

namespace SerapKeremGameTools._Game._PopUpSystem
{

    public abstract class PopUpManager : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] protected float animationDuration = 0.5f;
        [SerializeField] protected float hideDelay = 1f;

        // Abstract method to handle specific pop-up display logic
        protected abstract void ShowPopUp(Vector3 position, string text, float customDuration, PopUpAnimationType animationType);

        // Abstract method for handling animation logic (implemented in subclasses)
        protected abstract IEnumerator HandleAnimation(GameObject popUpObject, float duration, PopUpAnimationType animationType);

        // Common method to handle returning objects to the pool after a delay
        protected IEnumerator ReturnPopUpAfterDelay(GameObject popUpObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnPopUp(popUpObject);
        }

        // Placeholder method to be implemented by subclasses for returning the object
        protected abstract void ReturnPopUp(GameObject popUpObject);
    }
}
