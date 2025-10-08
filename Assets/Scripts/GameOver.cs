using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject panel;  
    [SerializeField] private Text scoreText;     
    [SerializeField] private Button restartBtn;  

    void Awake()
    {
        if (panel) panel.SetActive(false);
        if (restartBtn) restartBtn.onClick.AddListener(Rejouer);
    }

    public void Show(int finalScore)
    {
        if (panel) panel.SetActive(true);
        if (scoreText) scoreText.text = "Score final : " + finalScore;

        Time.timeScale = 0f;               // pause le jeu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Rejouer()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}