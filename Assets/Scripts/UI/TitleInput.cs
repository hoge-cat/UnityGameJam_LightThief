using System.Collections;
using UnityEngine;

public class TitleInput : MonoBehaviour
{
    [Header("シーン設定")]
    [SerializeField] private string mainSceneName = "Main";

    [Header("メニュー音")]
    [SerializeField] private TitleMenuSounds menuSounds;

    [Header("遷移設定")]
    [SerializeField] private float sceneLoadDelay = 0.2f;

    private bool isProcessing;

    public void StartGame()
    {
        if (isProcessing)
        {
            return;
        }

        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        isProcessing = true;

        menuSounds?.PlayDecide();

        yield return new WaitForSecondsRealtime(
            sceneLoadDelay
        );

        Time.timeScale = 1.0f;

        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.LoadScene(
                mainSceneName
            );
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                mainSceneName
            );
        }
    }

    public void QuitGame()
    {
        if (isProcessing)
        {
            return;
        }

        StartCoroutine(QuitGameCoroutine());
    }

    private IEnumerator QuitGameCoroutine()
    {
        isProcessing = true;

        menuSounds?.PlayDecide();

        yield return new WaitForSecondsRealtime(
            sceneLoadDelay
        );

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}