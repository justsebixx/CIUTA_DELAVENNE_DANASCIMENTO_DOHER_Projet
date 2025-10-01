using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 


public class GestionJeu : MonoBehaviour
{

    public static int score = 0;
    public Text scoreText;
    public float vitesse = 5f;
    void Update()
    {   

        scoreText.text = "Score : " + score;
        Vector2 deplacement = Vector2.zero;

        // ↑↓→← Pour se déplacer sur la map

        if (Input.GetKey(KeyCode.UpArrow))
        {
            deplacement += Vector2.up;
        }
        if (Input.GetKey(KeyCode.DownArrow)) 
        {
            deplacement += Vector2.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            deplacement += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow)) 
        {
            deplacement += Vector2.right;
        }

        transform.Translate(deplacement * vitesse * Time.deltaTime);
    }
    
    public static void AjoutScore(int point)
    {
        score += point;
    }
}
