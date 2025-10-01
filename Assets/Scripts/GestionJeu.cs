using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GestionJeu : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;
    public float tempsRestant = 60f;
    public Text timerText;

    void Update()
    {
        if (scoreText != null)
            scoreText.text = "Score : " + score;

        // mode de jeu avec temps
        tempsRestant -= Time.deltaTime;
        if (tempsRestant <= 0)
        {
            FinDePartie();
        }

        if (timerText != null)
            timerText.text = "Temps : " + Mathf.CeilToInt(tempsRestant);
    }

    public void AjoutScore(int points)
    {
        score += points;
        if (scoreText != null)
            scoreText.text = "Score : " + score;
    }

    public void FinDePartie()
    {
        Debug.Log("Fin du jeu");
        // TODO : remplacer par la scène de "Game Over" quand elle existera
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
