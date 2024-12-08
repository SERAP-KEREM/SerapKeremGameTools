using SerapKeremGameTools._Game._FPSPlayerSystem;
using UnityEngine;

public class TestInteractableObject : Interactable
{
    [Header("Test Interactable Object")]
    [SerializeField] private string messageOnFocus = "Odaklan?ld?!";
    [SerializeField] private string messageOnInteract = "Etkile?im Ba?lat?ld?!";
    [SerializeField] private Color focusColor = Color.yellow;
    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        
            gameObject.layer = 7; 
    }

    public override void OnInteract()
    {
        // Etkile?imde bir ?eyler yap?yoruz
        Debug.Log(messageOnInteract);
        // Örne?in nesnenin rengini de?i?tiriyoruz
        objectRenderer.material.color = Color.green;
    }

    public override void OnFocus()
    {
        // Odakland???nda bir ?eyler yap?yoruz
        Debug.Log(messageOnFocus);
        // Nesnenin rengini de?i?tiriyoruz
        objectRenderer.material.color = focusColor;
    }

    public override void OnLoseFocus()
    {
        // Odak kayboldu?unda bir ?eyler yap?yoruz
        Debug.Log("Odak kayboldu.");
        // Nesnenin rengini geri al?yoruz
        objectRenderer.material.color = Color.white;
    }
}
