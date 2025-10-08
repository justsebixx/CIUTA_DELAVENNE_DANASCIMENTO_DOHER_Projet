using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGameOver : MonoBehaviour
{
    public GestionJeu gestion;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("omagames"))
        {
            if (gestion != null) gestion.FinDePartie();
            Debug.Log("game ver");
        }
    }
}

