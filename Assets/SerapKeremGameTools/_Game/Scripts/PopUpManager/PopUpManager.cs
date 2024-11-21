using UnityEngine;
using System.Collections;
using SerapKeremGameTools._Game._Singleton;
using SerapKeremGameTools._Game._objectPool;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    // Base Manager for PopUp handling (common functionality for both PopUpTextManager and PopUpSpriteRendererManager)
    public interface PopUpManager
    {
        // ShowPopUp metodu, alt s?n?flar taraf?ndan özelle?tirilebilir
        public virtual void ShowPopUp(Vector3 position, string text = null, Sprite sprite = null, float customDuration = 0, PopUpAnimationType animationType = PopUpAnimationType.SlideDown)
        {
            // Bu metod genel bir i?lem yapmaz, alt s?n?flar bu metodu override eder
            // Örne?in, her bir PopUpTextManager veya PopUpSpriteRendererManager, ShowPopUp metodunu kendisine özel ?ekilde implement eder.
        }
    }
}
