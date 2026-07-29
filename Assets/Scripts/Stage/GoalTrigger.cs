using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    [Header("ゴール参照")]
    [SerializeField] private GoalDoor goalDoor;

    [Header("シーン設定")]
    [SerializeField] private string resultSceneName = "GameClear";

    private bool hasCleared;

    private void OnTriggerEnter(Collider other)
    {
        if (hasCleared)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (goalDoor == null || !goalDoor.IsUnlocked())
        {
            return;
        }

        hasCleared = true;
        Time.timeScale = 1.0f;

        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.LoadScene(
                resultSceneName);
        }
        else
        {
            SceneManager.LoadScene(
                resultSceneName);
        }
    }
}