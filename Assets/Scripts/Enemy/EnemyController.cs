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

    private EnemyState currentState;

    private void Start()
    {
        ChangeState(EnemyState.Patrol);
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

        if (!enemyVision.IsPlayerInDetectionDistance())
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    private void ChangeState(EnemyState newState)
    {
        currentState = newState;

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