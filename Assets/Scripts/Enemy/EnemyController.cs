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

        if (loseSightTimer >= loseSightTime)
        {
            ChangeState(EnemyState.Patrol);
        }
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