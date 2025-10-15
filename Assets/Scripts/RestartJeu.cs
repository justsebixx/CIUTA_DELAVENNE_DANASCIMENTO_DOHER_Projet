using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{

    [SerializeField] private string sceneDeJeu = "SampleScene";

    public void Recommencer()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneDeJeu);
    }

    public void Quitter()
    {
        Application.Quit();
    }
}
