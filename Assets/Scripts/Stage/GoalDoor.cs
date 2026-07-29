using UnityEngine;

public class GoalDoor : MonoBehaviour
{
    [Header("非表示にするドア")]
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    [Header("ゴール判定")]
    [SerializeField] private Collider goalTriggerCollider;

    private bool isUnlocked;

    private void Start()
    {
        LockGoal();
    }

    public void UnlockGoal()
    {
        if (isUnlocked)
        {
            return;
        }

        isUnlocked = true;

        // 左右の扉だけを消す
        if (leftDoor != null)
        {
            leftDoor.SetActive(false);
        }

        if (rightDoor != null)
        {
            rightDoor.SetActive(false);
        }

        // ゴール判定を有効化
        if (goalTriggerCollider != null)
        {
            goalTriggerCollider.enabled = true;
        }

        Debug.Log("ゴールが解放されました");
    }

    private void LockGoal()
    {
        isUnlocked = false;

        // 開始時は左右の扉を表示
        if (leftDoor != null)
        {
            leftDoor.SetActive(true);
        }

        if (rightDoor != null)
        {
            rightDoor.SetActive(true);
        }

        // 開始時はゴール判定を無効化
        if (goalTriggerCollider != null)
        {
            goalTriggerCollider.enabled = false;
        }
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }
}