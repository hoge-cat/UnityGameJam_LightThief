using UnityEngine;

public class SecurityCameraAlarm : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private SecurityCameraVision cameraVision;

    [Header("敵出現設定")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemySpawnPoint;

    private bool isAlarmActive;
    private bool hasSpawnedEnemy;

    private void Update()
    {
        if (isAlarmActive)
        {
            return;
        }

        if (cameraVision == null)
        {
            return;
        }

        /*
         * SecurityCameraVision側で、
         * 警戒時間を超えて赤色になったときだけ
         * CanSeePlayer()がtrueになる。
         */
        if (cameraVision.CanSeePlayer())
        {
            ActivateAlarm();
        }
    }

    private void ActivateAlarm()
    {
        if (isAlarmActive)
        {
            return;
        }

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

        if (enemyPrefab == null)
        {
            Debug.LogWarning(
                "Enemy Prefabが設定されていません");

            return;
        }

        if (enemySpawnPoint == null)
        {
            Debug.LogWarning(
                "Enemy Spawn Pointが設定されていません");

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