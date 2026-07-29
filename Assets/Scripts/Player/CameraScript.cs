using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [Header("追従設定")]
    public Transform target;

    public float distance = 5f;
    public float height = 2f;
    public float followSpeed = 10f;

    [Header("回転設定")]
    public float mouseSensitivity = 3.0f;
    public float cameraRotateSpeed = 120f;

    [Header("壁衝突設定")]
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private float cameraRadius = 0.25f;
    [SerializeField] private float wallOffset = 0.15f;
    [SerializeField] private float returnSpeed = 8.0f;

    private float yaw = 0f;
    private float pitch = 20f;

    private float currentDistance;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentDistance = distance;
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        RotateCamera();
    }

    private void LateUpdate()
    {
        FollowCamera();
    }

    private void RotateCamera()
    {
        Vector2 input = Vector2.zero;

        // 矢印キー
        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                input.x -= 1f;
            }

            if (Keyboard.current.rightArrowKey.isPressed)
            {
                input.x += 1f;
            }

            if (Keyboard.current.upArrowKey.isPressed)
            {
                input.y += 1f;
            }

            if (Keyboard.current.downArrowKey.isPressed)
            {
                input.y -= 1f;
            }
        }

        // コントローラー右スティック
        if (Gamepad.current != null)
        {
            Vector2 stick =
                Gamepad.current.rightStick.ReadValue();

            if (stick.magnitude >= 0.15f)
            {
                input += stick;
            }
        }

        // マウス
        Vector2 mouseInput = Vector2.zero;

        if (Mouse.current != null)
        {
            mouseInput =
                Mouse.current.delta.ReadValue();
        }

        yaw +=
            input.x *
            cameraRotateSpeed *
            Time.deltaTime;

        pitch -=
            input.y *
            cameraRotateSpeed *
            Time.deltaTime;

        yaw +=
            mouseInput.x *
            mouseSensitivity *
            0.02f;

        pitch -=
            mouseInput.y *
            mouseSensitivity *
            0.02f;

        pitch =
            Mathf.Clamp(
                pitch,
                -30f,
                70f);
    }

    private void FollowCamera()
    {
        if (target == null)
        {
            return;
        }

        Quaternion rotation =
            Quaternion.Euler(
                pitch,
                yaw,
                0f);

        Vector3 lookTarget =
            target.position +
            Vector3.up * 1.5f;

        Vector3 cameraDirection =
            rotation *
            new Vector3(
                0f,
                height,
                -distance);

        Vector3 desiredPosition =
            target.position +
            cameraDirection;

        Vector3 directionFromTarget =
            desiredPosition -
            lookTarget;

        float desiredDistance =
            directionFromTarget.magnitude;

        Vector3 normalizedDirection =
            directionFromTarget.normalized;

        float targetDistance =
            desiredDistance;

        RaycastHit hit;

        if (Physics.SphereCast(
            lookTarget,
            cameraRadius,
            normalizedDirection,
            out hit,
            desiredDistance,
            collisionLayer,
            QueryTriggerInteraction.Ignore))
        {
            targetDistance =
                Mathf.Max(
                    hit.distance - wallOffset,
                    0.3f);
        }

        float distanceChangeSpeed =
            targetDistance < currentDistance
                ? followSpeed
                : returnSpeed;

        currentDistance =
            Mathf.Lerp(
                currentDistance,
                targetDistance,
                distanceChangeSpeed *
                Time.deltaTime);

        Vector3 correctedPosition =
            lookTarget +
            normalizedDirection *
            currentDistance;

        transform.position =
            Vector3.Lerp(
                transform.position,
                correctedPosition,
                followSpeed *
                Time.deltaTime);

        transform.LookAt(lookTarget);
    }
}