using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleInput : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "Main";

    public void StartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(mainSceneName);
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