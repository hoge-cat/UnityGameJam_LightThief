using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadTitle()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Title");
    }

    public void LoadMain()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main");
    }

    public void LoadGameClear()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("GameClear");
    }

    public void LoadGameOver()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("GameOver");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}