using UnityEngine;

public class SecurityCameraAlarm : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private SecurityCameraVision cameraVision;

    [Header("警報設定")]
    [SerializeField] private float alarmDelay = 3.0f;

    [Header("敵出現設定")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemySpawnPoint;

    private float detectionTimer;
    private bool isAlarmActive;
    private bool hasSpawnedEnemy;

    private void Update()
    {
        if (isAlarmActive)
        {
            return;
        }

        if (cameraVision != null && cameraVision.CanSeePlayer())
        {
            detectionTimer += Time.deltaTime;

            if (detectionTimer >= alarmDelay)
            {
                ActivateAlarm();
            }
        }
        else
        {
            detectionTimer = 0.0f;
        }
    }

    private void ActivateAlarm()
    {
        isAlarmActive = true;

        Debug.Log("警報発動：敵を出現させます");

        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (hasSpawnedEnemy)
        {
            return;
        }

        if (enemyPrefab == null || enemySpawnPoint == null)
        {
            Debug.LogWarning("Enemy PrefabまたはEnemy Spawn Pointが設定されていません");
            return;
        }

        Instantiate(
            enemyPrefab,
            enemySpawnPoint.position,
            enemySpawnPoint.rotation);

        hasSpawnedEnemy = true;
    }

    public bool IsAlarmActive()
    {
        return isAlarmActive;
    }
}