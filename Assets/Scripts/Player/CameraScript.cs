using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    public Transform target;

    public float distance = 5f;
    public float height = 2f;
    public float mouseSensitivity = 3f;
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
        float keyX = 0;
        float keyY = 0;

        if (Input.GetKey(KeyCode.LeftArrow)) keyX = -1;
        if (Input.GetKey(KeyCode.RightArrow)) keyX = 1;
        if (Input.GetKey(KeyCode.UpArrow)) keyY = 1;
        if (Input.GetKey(KeyCode.DownArrow)) keyY = -1;

        // ゲームパッド右スティック
        float stickX = Input.GetAxis("RightStickX");
        float stickY = Input.GetAxis("RightStickY");

        // 全部合成
        float x = keyX + stickX;
        float y = keyY + stickY;

        yaw += x * mouseSensitivity;
        pitch -= y * mouseSensitivity;

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
