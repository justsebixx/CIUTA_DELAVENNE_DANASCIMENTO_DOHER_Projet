using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 


public class GestionJeu : MonoBehaviour
{

    public static int score = 0;
    public Text scoreText;

    // Mise à jour du score lorsqu'on entre en collision avec un objet
    void Update()
    {
        scoreText.text = "Score : " + score;
    }
    
    public static void AjoutScore(int point)
    {
        score += point;
    }
}
