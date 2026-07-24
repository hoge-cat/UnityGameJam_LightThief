using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectionDistance = 5.0f;
    [SerializeField] private float viewAngle = 90.0f;
    [SerializeField] private LayerMask obstacleLayer;

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
        if (player == null)
        {
            return false;
        }

        Vector3 rayOrigin =
            transform.position + Vector3.up * 1.0f;

        Vector3 playerTarget =
            player.position + Vector3.up * 1.0f;

        Vector3 directionToPlayer =
            playerTarget - rayOrigin;

        float distanceToPlayer =
            directionToPlayer.magnitude;

        if (distanceToPlayer > detectionDistance)
        {
            return false;
        }

        float angleToPlayer =
            Vector3.Angle(
                transform.forward,
                directionToPlayer.normalized);

        if (angleToPlayer > viewAngle * 0.5f)
        {
            return false;
        }

        bool hasObstacle =
            Physics.Raycast(
                rayOrigin,
                directionToPlayer.normalized,
                distanceToPlayer,
                obstacleLayer);

        if (hasObstacle)
        {
            return false;
        }

        return true;
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

        Vector3 origin =
            transform.position + Vector3.up * 0.1f;

        Vector3 leftDirection =
            Quaternion.Euler(
                0.0f,
                -viewAngle * 0.5f,
                0.0f)
            * transform.forward;

        Vector3 rightDirection =
            Quaternion.Euler(
                0.0f,
                viewAngle * 0.5f,
                0.0f)
            * transform.forward;

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
            float angle =
                Mathf.Lerp(
                    -viewAngle * 0.5f,
                    viewAngle * 0.5f,
                    i / (float)segmentCount);

            Vector3 direction =
                Quaternion.Euler(0.0f, angle, 0.0f)
                * transform.forward;

            Vector3 currentPoint =
                origin + direction * detectionDistance;

            Gizmos.DrawLine(
                previousPoint,
                currentPoint);

            previousPoint = currentPoint;
        }
    }
}