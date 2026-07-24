using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState
    {
        Patrol,
        Chase
    }

    [SerializeField] private EnemyPatrol enemyPatrol;
    [SerializeField] private EnemyVision enemyVision;
    [SerializeField] private EnemyChase enemyChase;

    [Header("開始状態")]
    [SerializeField] private bool startWithChase;

    [Header("見失い設定")]
    [SerializeField] private float loseSightTime = 3.0f;

    private EnemyState currentState;
    private float loseSightTimer;

    // 監視カメラから生成された敵か
    private bool isCameraSpawnedEnemy;

    // この敵を生成した監視カメラ
    private SecurityCameraAlarm sourceCameraAlarm;

    private void Start()
    {
        if (startWithChase)
        {
            ChangeState(EnemyState.Chase);
        }
        else
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                UpdatePatrolState();
                break;

            case EnemyState.Chase:
                UpdateChaseState();
                break;
        }
    }

    private void UpdatePatrolState()
    {
        enemyPatrol.UpdatePatrol();

        if (enemyVision.CanSeePlayer())
        {
            ChangeState(EnemyState.Chase);
        }
    }

    private void UpdateChaseState()
    {
        enemyChase.UpdateChase();

        if (enemyVision.CanSeePlayer())
        {
            loseSightTimer = 0.0f;
            return;
        }

        loseSightTimer += Time.deltaTime;

        if (loseSightTimer < loseSightTime)
        {
            return;
        }

        // 監視カメラから生成された敵なら、巡回せず消滅
        if (isCameraSpawnedEnemy)
        {
            FinishCameraChase();
            return;
        }

        ChangeState(EnemyState.Patrol);
    }

    public void StartChasing()
    {
        startWithChase = true;
        ChangeState(EnemyState.Chase);
    }

    // 監視カメラから生成された敵として初期化
    public void InitializeAsCameraSpawned(
        SecurityCameraAlarm cameraAlarm)
    {
        isCameraSpawnedEnemy = true;
        sourceCameraAlarm = cameraAlarm;
        startWithChase = true;

        ChangeState(EnemyState.Chase);
    }

    private void FinishCameraChase()
    {
        enemyChase.StopChase();

        if (sourceCameraAlarm != null)
        {
            sourceCameraAlarm.OnSpawnedEnemyFinished();
        }

        Destroy(gameObject);
    }

    private void ChangeState(EnemyState newState)
    {
        currentState = newState;
        loseSightTimer = 0.0f;

        switch (currentState)
        {
            case EnemyState.Patrol:
                enemyChase.StopChase();
                enemyPatrol.StartPatrol();
                break;

            case EnemyState.Chase:
                enemyPatrol.StopPatrol();
                break;
        }
    }
}