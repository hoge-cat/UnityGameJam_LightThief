using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadTitle()
    {
        LoadScene("Title");
    }

    public void LoadMain()
    {
        LoadScene("Main");
    }

    public void LoadGameClear()
    {
        LoadScene("GameClear");
    }

    public void LoadGameOver()
    {
        LoadScene("GameOver");
    }

    private void LoadScene(string sceneName)
    {
        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.LoadScene(sceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
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