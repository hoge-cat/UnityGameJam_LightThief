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

    [Header("ライトによる発見距離補正")]
    [SerializeField] private float lightOnMultiplier = 1.5f;
    [SerializeField] private float lightOffMultiplier = 0.6f;

    [Header("障害物設定")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("警戒設定")]
    [SerializeField] private float alertTime = 1.0f;

    private FlashlightController flashlightController;

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

        if (player != null)
        {
            flashlightController =
                player.GetComponentInChildren<FlashlightController>();

            if (flashlightController == null)
            {
                Debug.LogWarning(
                    "SecurityCameraVision：FlashlightControllerが見つかりません。");
            }
        }
    }

    private void Update()
    {
        float currentDetectionDistance =
            GetCurrentDetectionDistance();

        if (viewConeVisualizer != null)
        {
            viewConeVisualizer.SetViewSettings(
                currentDetectionDistance,
                viewAngle);
        }

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
            // 一瞬判定が途切れても、警戒時間を即座に0へ戻さない
            detectionTimer -= Time.deltaTime * 2.0f;
            detectionTimer = Mathf.Max(0.0f, detectionTimer);

            canSeePlayer = false;

            if (viewConeVisualizer != null)
            {
                if (detectionTimer > 0.0f)
                {
                    viewConeVisualizer.SetViewState(
                        ViewConeVisualizer.ViewState.Warning);
                }
                else
                {
                    viewConeVisualizer.SetViewState(
                        ViewConeVisualizer.ViewState.Normal);
                }
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

        // プレイヤーの足元ではなく、胴体付近を見る
        Vector3 playerTarget =
            player.position + Vector3.up * 1.0f;

        Vector3 directionToPlayer =
            playerTarget - visionOrigin.position;

        // 距離・角度判定では高さを無視
        Vector3 horizontalDirectionToPlayer =
            new Vector3(
                directionToPlayer.x,
                0.0f,
                directionToPlayer.z);

        float horizontalDistance =
            horizontalDirectionToPlayer.magnitude;

        float currentDetectionDistance =
            GetCurrentDetectionDistance();

        if (horizontalDistance >
            currentDetectionDistance)
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
                visionOrigin.forward.z);

        if (horizontalForward.sqrMagnitude <= 0.001f)
        {
            return false;
        }

        horizontalForward.Normalize();

        float angleToPlayer =
            Vector3.Angle(
                horizontalForward,
                horizontalDirectionToPlayer.normalized);

        if (angleToPlayer > viewAngle * 0.5f)
        {
            return false;
        }

        float actualDistance =
            directionToPlayer.magnitude;

        Vector3 actualDirection =
            directionToPlayer.normalized;

        // プレイヤーの胴体へRayを飛ばして障害物を確認
        if (Physics.Raycast(
            visionOrigin.position,
            actualDirection,
            out RaycastHit hit,
            actualDistance,
            obstacleLayer,
            QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        return true;
    }

    private float GetCurrentDetectionDistance()
    {
        if (flashlightController == null)
        {
            return detectionDistance;
        }

        if (flashlightController.IsLightOn())
        {
            return detectionDistance * lightOnMultiplier;
        }

        return detectionDistance * lightOffMultiplier;
    }

    private void OnDrawGizmosSelected()
    {
        if (visionOrigin == null)
        {
            return;
        }

        float currentDetectionDistance =
            Application.isPlaying
                ? GetCurrentDetectionDistance()
                : detectionDistance;

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
            origin + leftDirection * currentDetectionDistance);

        Gizmos.DrawLine(
            origin,
            origin + rightDirection * currentDetectionDistance);

        const int segmentCount = 30;

        Vector3 previousPoint =
            origin + leftDirection * currentDetectionDistance;

        for (int i = 1; i <= segmentCount; i++)
        {
            float angle =
                Mathf.Lerp(
                    -viewAngle * 0.5f,
                    viewAngle * 0.5f,
                    i / (float)segmentCount);

            Vector3 direction =
                Quaternion.Euler(0.0f, angle, 0.0f)
                * visionOrigin.forward;

            Vector3 currentPoint =
                origin + direction * currentDetectionDistance;

            Gizmos.DrawLine(
                previousPoint,
                currentPoint);

            previousPoint = currentPoint;
        }

        if (Application.isPlaying && player != null)
        {
            Gizmos.color =
                canSeePlayer
                    ? Color.red
                    : Color.gray;

            Gizmos.DrawLine(
                visionOrigin.position,
                player.position);
        }
    }
}