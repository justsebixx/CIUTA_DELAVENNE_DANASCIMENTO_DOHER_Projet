using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Collecter : MonoBehaviour
{

    public static int points = 1;

    /* Sous-programme appelé lors d'une collision : si l'objet touché est une pomme,
     on ajoute les points au score et on détruit la pomme du jeu*/

    private void Collision(Collider2D other)
    {
        if (other.CompareTag("Pomme"))
        {
            GestionJeu.AjoutScore(points);
            Destroy(gameObject);
        }
    }
}
