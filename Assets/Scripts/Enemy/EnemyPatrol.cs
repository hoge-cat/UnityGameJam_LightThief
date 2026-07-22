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
        if (!HasValidPatrolPoints())
        {
            isPatrolling = false;
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

        if (!HasValidPatrolPoints())
        {
            isPatrolling = false;
            return;
        }

        if (agent == null || !agent.isOnNavMesh)
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

    private bool HasValidPatrolPoints()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            return false;
        }

        foreach (Transform patrolPoint in patrolPoints)
        {
            if (patrolPoint == null)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveToCurrentPoint()
    {
        if (!HasValidPatrolPoints())
        {
            return;
        }

        if (agent == null || !agent.isOnNavMesh)
        {
            return;
        }

        agent.SetDestination(patrolPoints[currentPointIndex].position);
    }
}