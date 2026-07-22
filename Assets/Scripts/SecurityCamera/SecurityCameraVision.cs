using UnityEngine;

public class SecurityCameraVision : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private Transform visionOrigin;
    [SerializeField] private Transform player;

    [Header("視野設定")]
    [SerializeField] private float detectionDistance = 8.0f;
    [SerializeField, Range(0.0f, 360.0f)]
    private float viewAngle = 60.0f;

    [Header("障害物設定")]
    [SerializeField] private LayerMask obstacleLayer;

    private bool canSeePlayer;

    private void Update()
    {
        canSeePlayer = CheckCanSeePlayer();
    }

    public bool CanSeePlayer()
    {
        return canSeePlayer;
    }

    private bool CheckCanSeePlayer()
    {
        if (visionOrigin == null || player == null)
        {
            return false;
        }

        Vector3 directionToPlayer =
            player.position - visionOrigin.position;

        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > detectionDistance)
        {
            return false;
        }

        Vector3 normalizedDirection =
            directionToPlayer.normalized;

        float angleToPlayer = Vector3.Angle(
            visionOrigin.forward,
            normalizedDirection);

        if (angleToPlayer > viewAngle * 0.5f)
        {
            return false;
        }

        if (Physics.Raycast(
            visionOrigin.position,
            normalizedDirection,
            distanceToPlayer,
            obstacleLayer))
        {
            return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (visionOrigin == null)
        {
            return;
        }

        Gizmos.color =
            Application.isPlaying && canSeePlayer
            ? Color.red
            : Color.yellow;

        Vector3 origin =
            visionOrigin.position;

        Vector3 leftDirection =
            Quaternion.Euler(
                0.0f,
                -viewAngle * 0.5f,
                0.0f)
            * visionOrigin.forward;

        Vector3 rightDirection =
            Quaternion.Euler(
                0.0f,
                viewAngle * 0.5f,
                0.0f)
            * visionOrigin.forward;

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
                Quaternion.Euler(0.0f, angle, 0.0f)
                * visionOrigin.forward;

            Vector3 currentPoint =
                origin + direction * detectionDistance;

            Gizmos.DrawLine(
                previousPoint,
                currentPoint);

            previousPoint = currentPoint;
        }

        if (Application.isPlaying && player != null)
        {
            Gizmos.color = canSeePlayer
                ? Color.red
                : Color.gray;

            Gizmos.DrawLine(
                visionOrigin.position,
                player.position);
        }
    }
}