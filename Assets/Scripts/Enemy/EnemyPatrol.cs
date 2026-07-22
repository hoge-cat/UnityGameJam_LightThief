using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float arrivalDistance = 0.5f;

    private NavMeshAgent agent;
    private int currentPointIndex;
    private bool isPatrolling;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartPatrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogWarning("巡回ポイントが設定されていません。");
            return;
        }

        isPatrolling = true;
        MoveToCurrentPoint();
    }

    public void UpdatePatrol()
    {
        if (!isPatrolling)
        {
            return;
        }

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            return;
        }

        if (agent.pathPending)
        {
            return;
        }

        if (agent.remainingDistance <= arrivalDistance)
        {
            currentPointIndex++;

            if (currentPointIndex >= patrolPoints.Length)
            {
                currentPointIndex = 0;
            }

            MoveToCurrentPoint();
        }
    }

    public void StopPatrol()
    {
        isPatrolling = false;
    }

    private void MoveToCurrentPoint()
    {
        agent.SetDestination(patrolPoints[currentPointIndex].position);
    }
}