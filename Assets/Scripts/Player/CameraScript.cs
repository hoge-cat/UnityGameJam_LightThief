using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    public Transform target;

    public float distance = 5f;
    public float height = 2f;
    public float mouseSensitivity = 3.0f;
    public float cameraRotateSpeed = 120f;
    public float followSpeed = 10f;

    float yaw = 0f;
    float pitch = 20f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        RotateCamera();
    }

    void LateUpdate()
    {
        FollowCamera();
    }

    void RotateCamera()
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
            Vector2 stick = Gamepad.current.rightStick.ReadValue();

            if (stick.magnitude >= 0.15f)
            {
                input += stick;
            }
        }

        // マウス
        Vector2 mouseInput = Vector2.zero;

        if (Mouse.current != null)
        {
            mouseInput = Mouse.current.delta.ReadValue();
        }

        // 矢印キー・右スティック
        yaw += input.x * cameraRotateSpeed * Time.deltaTime;
        pitch -= input.y * cameraRotateSpeed * Time.deltaTime;

        // マウス
        yaw += mouseInput.x * mouseSensitivity * 0.02f;
        pitch -= mouseInput.y * mouseSensitivity * 0.02f;

        pitch = Mathf.Clamp(pitch, -30f, 70f);
    }

    void FollowCamera()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 offset = rotation * new Vector3(0, height, -distance);

        Vector3 targetPos = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            followSpeed * Time.deltaTime);

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
