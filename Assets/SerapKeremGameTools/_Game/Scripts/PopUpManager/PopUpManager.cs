using UnityEngine;
using System.Collections;
using SerapKeremGameTools._Game._Singleton;
using SerapKeremGameTools._Game._objectPool;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    // Base Manager for PopUp handling (common functionality for both PopUpTextManager and PopUpSpriteRendererManager)
    public interface IPopUpManager
    {
        void ShowPopUp(Vector3 position, string text = null, Sprite sprite = null, float customDuration = 0, PopUpAnimationType animationType = PopUpAnimationType.SlideDown);
    }
   
}
