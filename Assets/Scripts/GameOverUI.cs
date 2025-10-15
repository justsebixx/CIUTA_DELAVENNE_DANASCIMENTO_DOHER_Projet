
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Références UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI score; 
    [SerializeField] private Button restartBtn;

    private bool shown = false;
    private bool prevCursorVisible;
    private CursorLockMode prevCursorLock;

    private void Awake()
    {
        if (panel) panel.SetActive(false);
    }

    private void OnEnable()
    {
        if (restartBtn) restartBtn.onClick.AddListener(Rejouer);
    }

    private void OnDisable()
    {
        if (restartBtn) restartBtn.onClick.RemoveListener(Rejouer);
    }

    public void Show(int finalScore)
    {
        if (shown) return;
        shown = true;

        if (panel) panel.SetActive(true);
        if (score) score.text = $"Score final : {finalScore}";

        Time.timeScale = 0f;

        prevCursorVisible = Cursor.visible;
        prevCursorLock = Cursor.lockState;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Rejouer()
    {
        Time.timeScale = 1f;
        Cursor.visible = prevCursorVisible;
        Cursor.lockState = prevCursorLock;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
