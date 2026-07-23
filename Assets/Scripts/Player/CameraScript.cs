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
        RotateCamera();
    }

    void LateUpdate()
    {
        FollowCamera();
    }

    void RotateCamera()
    {
        float keyX = 0f;
        float keyY = 0f;

        if (Keyboard.current.leftArrowKey.isPressed) keyX = -1;
        if (Keyboard.current.rightArrowKey.isPressed) keyX = 1;
        if (Keyboard.current.upArrowKey.isPressed) keyY = 1;
        if (Keyboard.current.downArrowKey.isPressed) keyY = -1;

        float stickX = 0f;
        float stickY = 0f;

        if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.rightStick.ReadValue();
            stickX = stick.x;
            stickY = stick.y;
        }

        float x = keyX + stickX;
        float y = keyY + stickY;

        // カメラ回転
        yaw += x * cameraRotateSpeed * Time.deltaTime;
        pitch -= y * cameraRotateSpeed * Time.deltaTime;

        // 上下の回転制限
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
