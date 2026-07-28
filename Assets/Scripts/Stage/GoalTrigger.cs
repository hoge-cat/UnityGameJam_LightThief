using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private string resultSceneName = "GameClear";

    private bool hasReachedGoal;

    private void OnTriggerEnter(Collider other)
    {
        if (hasReachedGoal)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        hasReachedGoal = true;

        Debug.Log("ゴールしました");

        Time.timeScale = 1.0f;
        SceneManager.LoadScene(resultSceneName);
    }
}