using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerGameOver : MonoBehaviour
{
    [Header("Détection")]
    [SerializeField] private string tagCible = "shrek";
    [Header("Référence Jeu")]
    [SerializeField] private GestionJeu gestion;

    private bool dejaDeclenche = false;

    private void Reset()
    {
        if (gestion == null) gestion = FindObjectOfType<GestionJeu>();
    }

    private void Awake()
    {
        if (gestion == null) gestion = FindObjectOfType<GestionJeu>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dejaDeclenche) return;
        if (!other.CompareTag(tagCible)) return;

        dejaDeclenche = true;

        if (gestion != null)
        {
            gestion.FinDePartie();               
            Debug.Log("[TriggerGameOver] Game Over déclenché.");
        }
        else
        {
            Debug.LogWarning("[TriggerGameOver] GestionJeu manquant, fallback -> reload scène.");
            Time.timeScale = 1f;
            SceneManager.LoadScene("GameOver");
        }
    }
}
