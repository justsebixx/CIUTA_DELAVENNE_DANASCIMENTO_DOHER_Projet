// en haut
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GestionJeu : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;
    public float tempsRestant = 60f;
    public Text timerText;

    // AJOUT:
    [Header("Lien Game Over")]
    public GameOverUI gameOverUI; 
    private bool finDeclaree = false; 

    void Update()
    {
        if (scoreText != null)
            scoreText.text = "Score : " + score;

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
        // MODIF:
        if (finDeclaree) return;
        finDeclaree = true;

        Debug.Log("Fin du jeu");

        
        if (gameOverUI != null)
        {
            // Etre sur que le timer n’est pas négatif
            if (tempsRestant < 0f) tempsRestant = 0f;

            gameOverUI.Show(score);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
