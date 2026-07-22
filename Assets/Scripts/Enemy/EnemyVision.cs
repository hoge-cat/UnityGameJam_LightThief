using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectionDistance = 5.0f;
    [SerializeField] private float viewAngle = 90.0f;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogWarning(
                    "EnemyVision：Playerタグのオブジェクトが見つかりません。");
            }
        }
    }

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

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying && CanSeePlayer())
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        Vector3 origin = transform.position + Vector3.up * 0.1f;

        Vector3 leftDirection =
            Quaternion.Euler(0.0f, -viewAngle * 0.5f, 0.0f) * transform.forward;

        Vector3 rightDirection =
            Quaternion.Euler(0.0f, viewAngle * 0.5f, 0.0f) * transform.forward;

        Gizmos.DrawLine(
            origin,
            origin + leftDirection * detectionDistance);

        Gizmos.DrawLine(
            origin,
            origin + rightDirection * detectionDistance);

        const int segmentCount = 30;

        Vector3 previousPoint =
            origin + leftDirection * detectionDistance;

        for (int i = 1; i <= segmentCount; i++)
        {
            float angle = Mathf.Lerp(
                -viewAngle * 0.5f,
                viewAngle * 0.5f,
                i / (float)segmentCount);

            Vector3 direction =
                Quaternion.Euler(0.0f, angle, 0.0f) * transform.forward;

            Vector3 currentPoint =
                origin + direction * detectionDistance;

            Gizmos.DrawLine(previousPoint, currentPoint);

            previousPoint = currentPoint;
        }
    }
}