using System.Collections.Generic;
using UnityEngine;
namespace SerapKeremGameTools._Game._FPSPlayerSystem
{
    public abstract class Interactable : MonoBehaviour
{
    private void Awake()
    {
        gameObject.layer = 9;
    }

    public abstract void OnInteract();
    public abstract void OnFocus();
    public abstract void OnLoseFocus();
}
}