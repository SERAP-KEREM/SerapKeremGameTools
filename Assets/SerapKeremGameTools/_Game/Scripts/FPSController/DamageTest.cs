using SerapKeremGameTools._Game._FPSPlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            FPSController.OnTakeDamage(15);
    }
}
