using UnityEngine;

public class SecurityCameraVision : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private Transform visionOrigin;
    [SerializeField] private Transform player;
    [SerializeField] private ViewConeVisualizer viewConeVisualizer;

    [Header("視野設定")]
    [SerializeField] private float detectionDistance = 8.0f;
    [SerializeField, Range(0.0f, 360.0f)]
    private float viewAngle = 60.0f;

    [Header("障害物設定")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("警戒設定")]
    [SerializeField] private float alertTime = 1.0f;

    private float detectionTimer;

    private bool canSeePlayer;

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
                    "SecurityCameraVision：Playerタグのオブジェクトが見つかりません。");
            }
        }
    }

    private void Update()
    {
        bool isPlayerVisible = CheckCanSeePlayer();

        if (isPlayerVisible)
        {
            detectionTimer += Time.deltaTime;

            if (detectionTimer >= alertTime)
            {
                canSeePlayer = true;

                if (viewConeVisualizer != null)
                {
                    viewConeVisualizer.SetViewState(
                        ViewConeVisualizer.ViewState.Alert);
                }
            }
            else
            {
                canSeePlayer = false;

                if (viewConeVisualizer != null)
                {
                    viewConeVisualizer.SetViewState(
                        ViewConeVisualizer.ViewState.Warning);
                }
            }
        }
        else
        {
            detectionTimer = 0.0f;
            canSeePlayer = false;

            if (viewConeVisualizer != null)
            {
                viewConeVisualizer.SetViewState(
                    ViewConeVisualizer.ViewState.Normal);
            }
        }
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

    /*
     * 距離と角度の判定では高さを無視する。
     * 床に表示している扇形と同じ水平面で判定するため。
     */
    Vector3 horizontalDirectionToPlayer =
        new Vector3(
            directionToPlayer.x,
            0.0f,
            directionToPlayer.z);

    float horizontalDistance =
        horizontalDirectionToPlayer.magnitude;

    if (horizontalDistance > detectionDistance)
    {
        return false;
    }

    if (horizontalDistance <= 0.01f)
    {
        return true;
    }

    Vector3 horizontalForward =
        new Vector3(
            visionOrigin.forward.x,
            0.0f,
            visionOrigin.forward.z).normalized;

    Vector3 normalizedHorizontalDirection =
        horizontalDirectionToPlayer.normalized;

    float angleToPlayer = Vector3.Angle(
        horizontalForward,
        normalizedHorizontalDirection);

    if (angleToPlayer > viewAngle * 0.5f)
    {
        return false;
    }

    /*
     * 障害物判定は実際の高さを使う。
     * カメラからプレイヤーへ直接Rayを飛ばす。
     */
    float actualDistance =
        directionToPlayer.magnitude;

    Vector3 actualDirection =
        directionToPlayer.normalized;

    if (Physics.Raycast(
        visionOrigin.position,
        actualDirection,
        actualDistance,
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