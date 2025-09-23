using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Collecter : MonoBehaviour
{

    public static int points = 1;

    /* Sous-programme appel� lors d'une collision : si l'objet touch� est une pomme,
     on ajoute les points au score et on d�truit la pomme du jeu*/

    private void Collision(Collider2D other)
    {
        if (other.CompareTag("Pomme"))
        {
            GestionJeu.AjoutScore(points);
            Destroy(gameObject);
        }
    }
}
