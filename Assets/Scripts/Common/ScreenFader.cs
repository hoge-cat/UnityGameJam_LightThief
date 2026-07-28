using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeTime = 1.0f;

    private bool isFading;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadScene(string sceneName)
    {
        if (isFading)
        {
            return;
        }

        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    private IEnumerator FadeIn()
    {
        if (fadeImage == null)
        {
            yield break;
        }

        isFading = true;

        fadeImage.gameObject.SetActive(true);

        Color color = fadeImage.color;
        color.a = 1.0f;
        fadeImage.color = color;

        float elapsedTime = 0.0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;

            color.a = Mathf.Lerp(
                1.0f,
                0.0f,
                elapsedTime / fadeTime
            );

            fadeImage.color = color;

            yield return null;
        }

        color.a = 0.0f;
        fadeImage.color = color;

        fadeImage.gameObject.SetActive(false);
        isFading = false;
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        if (fadeImage == null)
        {
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        isFading = true;

        fadeImage.gameObject.SetActive(true);

        Color color = fadeImage.color;
        color.a = 0.0f;
        fadeImage.color = color;

        float elapsedTime = 0.0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;

            color.a = Mathf.Lerp(
                0.0f,
                1.0f,
                elapsedTime / fadeTime
            );

            fadeImage.color = color;

            yield return null;
        }

        color.a = 1.0f;
        fadeImage.color = color;

        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneName);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}