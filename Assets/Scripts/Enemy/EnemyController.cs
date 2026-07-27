using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private EnemyAnimator enemyAnimator;

    [Header("攻撃設定")]
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float attackInterval = 1.0f;
    [SerializeField] private float attackBatteryDamage = 20.0f;

    [SerializeField] private BatteryManager batteryManager;

    private float attackTimer;

    [Header("開始状態")]
    [SerializeField] private bool startWithChase;

    [Header("見失い設定")]
    [SerializeField] private float loseSightTime = 3.0f;

    [Header("移動速度")]
    [SerializeField] private float patrolSpeed = 1.5f;
    [SerializeField] private float chaseSpeed = 3.5f;

    private NavMeshAgent agent;

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

        UpdateAnimation();
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
        bool canSeePlayer =
            enemyVision.CanSeePlayer();

        if (canSeePlayer)
        {
            loseSightTimer = 0.0f;
        }
        else
        {
            loseSightTimer += Time.deltaTime;

            if (loseSightTimer >= loseSightTime)
            {
                if (isCameraSpawnedEnemy)
                {
                    FinishCameraChase();
                    return;
                }

                ChangeState(EnemyState.Patrol);
                return;
            }
        }

        float distanceToPlayer =
            enemyVision.GetDistanceToPlayer();

        // プレイヤーが見えていて、攻撃距離内の場合だけ攻撃
        if (canSeePlayer &&
            distanceToPlayer <= attackDistance)
        {
            enemyChase.StopChase();

            attackTimer += Time.deltaTime;

            if (attackTimer >= attackInterval)
            {
                attackTimer = 0.0f;

                if (enemyAnimator != null)
                {
                    enemyAnimator.PlayAttack();
                }

                if (batteryManager != null)
                {
                    batteryManager.DrainBattery(
                        attackBatteryDamage);
                }

                Debug.Log("Enemyが攻撃しました");
            }

            return;
        }

        attackTimer = 0.0f;
        enemyChase.UpdateChase();
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

    private void UpdateAnimation()
    {
        if (enemyAnimator == null || agent == null)
        {
            return;
        }

        float speed = 0.0f;

        if (agent.isOnNavMesh)
        {
            speed = agent.velocity.magnitude;
        }

        enemyAnimator.SetSpeed(speed);
    }

    private void ChangeState(EnemyState newState)
    {
        currentState = newState;
        loseSightTimer = 0.0f;

        switch (currentState)
        {
            case EnemyState.Patrol:
                if (agent != null)
                {
                    agent.speed = patrolSpeed;
                }

                enemyChase.StopChase();
                enemyPatrol.StartPatrol();
                break;

            case EnemyState.Chase:
                if (agent != null)
                {
                    agent.speed = chaseSpeed;
                }

                enemyPatrol.StopPatrol();
                break;
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (batteryManager == null)
        {
            batteryManager =
                FindFirstObjectByType<BatteryManager>();
        }
    }
}