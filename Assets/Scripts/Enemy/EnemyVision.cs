using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectionDistance = 5.0f;
    [SerializeField] private float viewAngle = 90.0f;

    public bool CanSeePlayer()
    {
        if (!IsPlayerInDetectionDistance())
        {
            return false;
        }

        Vector3 directionToPlayer =
            (player.position - transform.position).normalized;

        float angleToPlayer =
            Vector3.Angle(transform.forward, directionToPlayer);

        return angleToPlayer <= viewAngle * 0.5f;
    }

    public bool IsPlayerInDetectionDistance()
    {
        if (player == null)
        {
            return false;
        }

        float distanceToPlayer =
            Vector3.Distance(transform.position, player.position);

        return distanceToPlayer <= detectionDistance;
    }
}