using UnityEngine;
using UnityEngine.AI;

public class SecurityCameraAlarm : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private SecurityCameraVision cameraVision;
    [SerializeField] private Transform player;

    [Header("敵出現設定")]
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField]
    private float spawnDistance = 5.0f;

    [SerializeField]
    private float navMeshSearchDistance = 2.0f;

    [Header("出現場所の確認")]
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField]
    private float lineCheckHeight = 1.0f;

    private bool isAlarmActive;
    private bool hasSpawnedEnemy;

    // 敵が消えた直後に連続出現しないためのフラグ
    private bool waitingForPlayerToLeave;

    private void Start()
    {
        if (player != null)
        {
            return;
        }

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning(
                "SecurityCameraAlarm：Playerが見つかりません");
        }
    }

    private void Update()
    {
        if (cameraVision == null)
        {
            return;
        }

        /*
         * 生成した敵が消えた後は、
         * プレイヤーが一度カメラの視界から出るまで待つ。
         */
        if (waitingForPlayerToLeave)
        {
            if (!cameraVision.CanSeePlayer())
            {
                waitingForPlayerToLeave = false;
                isAlarmActive = false;
            }

            return;
        }

        if (isAlarmActive || hasSpawnedEnemy)
        {
            return;
        }

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

        Debug.Log(
            "警報発動：敵を出現させます");

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

            ResetAlarmAfterSpawnFailure();
            return;
        }

        if (player == null)
        {
            Debug.LogWarning(
                "Playerが設定されていません");

            ResetAlarmAfterSpawnFailure();
            return;
        }

        if (!TryFindSpawnPosition(
            out Vector3 spawnPosition))
        {
            Debug.LogWarning(
                "壁を挟まない出現可能地点が見つかりません");

            ResetAlarmAfterSpawnFailure();
            return;
        }

        Vector3 directionToPlayer =
            player.position - spawnPosition;

        directionToPlayer.y = 0.0f;

        Quaternion spawnRotation =
            Quaternion.identity;

        if (directionToPlayer.sqrMagnitude > 0.001f)
        {
            spawnRotation =
                Quaternion.LookRotation(
                    directionToPlayer.normalized);
        }

        GameObject spawnedEnemy =
            Instantiate(
                enemyPrefab,
                spawnPosition,
                spawnRotation);

        EnemyController enemyController =
            spawnedEnemy.GetComponent<EnemyController>();

        if (enemyController == null)
        {
            enemyController =
                spawnedEnemy.GetComponentInChildren<EnemyController>();
        }

        if (enemyController != null)
        {
            enemyController.InitializeAsCameraSpawned(this);
        }
        else
        {
            Debug.LogWarning(
                "生成したEnemyにEnemyControllerが見つかりません");
        }

        hasSpawnedEnemy = true;
    }

    private bool TryFindSpawnPosition(
        out Vector3 spawnPosition)
    {
        spawnPosition = Vector3.zero;

        /*
         * プレイヤーの後方から順番に、
         * 左右や前方も候補として確認する。
         */
        float[] angleOffsets =
        {
            180.0f,
            135.0f,
            -135.0f,
            90.0f,
            -90.0f,
            45.0f,
            -45.0f,
            0.0f
        };

        foreach (float angleOffset in angleOffsets)
        {
            Vector3 direction =
                Quaternion.Euler(
                    0.0f,
                    angleOffset,
                    0.0f)
                * player.forward;

            direction.y = 0.0f;
            direction.Normalize();

            Vector3 desiredPosition =
                player.position
                + direction * spawnDistance;

            if (!NavMesh.SamplePosition(
                desiredPosition,
                out NavMeshHit navMeshHit,
                navMeshSearchDistance,
                NavMesh.AllAreas))
            {
                continue;
            }

            Vector3 candidatePosition =
                navMeshHit.position;

            // プレイヤーと出現地点の間に壁があるなら不採用
            if (IsWallBetweenPlayerAndPosition(
                candidatePosition))
            {
                continue;
            }

            // NavMesh上でプレイヤーまで到達できるか確認
            if (!CanReachPlayer(candidatePosition))
            {
                continue;
            }

            spawnPosition = candidatePosition;
            return true;
        }

        return false;
    }

    private bool IsWallBetweenPlayerAndPosition(
        Vector3 candidatePosition)
    {
        Vector3 playerCheckPosition =
            player.position
            + Vector3.up * lineCheckHeight;

        Vector3 spawnCheckPosition =
            candidatePosition
            + Vector3.up * lineCheckHeight;

        return Physics.Linecast(
            playerCheckPosition,
            spawnCheckPosition,
            obstacleLayer);
    }

    private bool CanReachPlayer(
        Vector3 candidatePosition)
    {
        if (!NavMesh.SamplePosition(
            player.position,
            out NavMeshHit playerNavMeshHit,
            navMeshSearchDistance,
            NavMesh.AllAreas))
        {
            return false;
        }

        NavMeshPath path =
            new NavMeshPath();

        bool pathFound =
            NavMesh.CalculatePath(
                candidatePosition,
                playerNavMeshHit.position,
                NavMesh.AllAreas,
                path);

        return pathFound
            && path.status
            == NavMeshPathStatus.PathComplete;
    }

    public void OnSpawnedEnemyFinished()
    {
        hasSpawnedEnemy = false;

        /*
         * この時点ではすぐ再出現させず、
         * プレイヤーが一度視界から出るまで待つ。
         */
        waitingForPlayerToLeave = true;

        Debug.Log(
            "追跡終了：生成した敵が消滅しました");
    }

    private void ResetAlarmAfterSpawnFailure()
    {
        isAlarmActive = false;
        hasSpawnedEnemy = false;
    }

    public bool IsAlarmActive()
    {
        return isAlarmActive;
    }
}