using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float dashSpeed = 5.0f;
    [SerializeField] private float jumpPower = 5.0f;

    private Rigidbody rb;
    private Camera mainCamera;

    private Vector2 moveInput;
    private bool isDash;
    private bool isGround = true;
    private bool jumpRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        // プレイヤーが物理演算で倒れないようにする
        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        ReadMoveInput();
        ReadJumpInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        JumpPlayer();
    }

    private void ReadMoveInput()
    {
        moveInput = Vector2.zero;
        isDash = false;

        // キーボード
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
            {
                moveInput.y += 1.0f;
            }

            if (Keyboard.current.sKey.isPressed)
            {
                moveInput.y -= 1.0f;
            }

            if (Keyboard.current.dKey.isPressed)
            {
                moveInput.x += 1.0f;
            }

            if (Keyboard.current.aKey.isPressed)
            {
                moveInput.x -= 1.0f;
            }

            isDash =
                Keyboard.current.leftShiftKey.isPressed ||
                Keyboard.current.rightShiftKey.isPressed;
        }

        // ゲームパッド
        if (Gamepad.current != null)
        {
            Vector2 stickInput = Gamepad.current.leftStick.ReadValue();

            if (moveInput.sqrMagnitude <= 0.01f)
            {
                moveInput = stickInput;
            }

            if (Gamepad.current.buttonWest.isPressed)
            {
                isDash = true;
            }
        }

        moveInput = Vector2.ClampMagnitude(moveInput, 1.0f);
    }

    private void ReadJumpInput()
    {
        bool keyboardJump =
            Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame;

        bool gamepadJump =
            Gamepad.current != null &&
            Gamepad.current.buttonSouth.wasPressedThisFrame;

        if (isGround && (keyboardJump || gamepadJump))
        {
            jumpRequested = true;
        }
    }

    private void MovePlayer()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;

            if (mainCamera == null)
            {
                return;
            }
        }

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        // カメラの上下方向を無視
        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection =
            cameraForward * moveInput.y +
            cameraRight * moveInput.x;

        float currentSpeed = isDash ? dashSpeed : moveSpeed;

        Vector3 nextPosition =
            rb.position +
            moveDirection * currentSpeed * Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);

        // 移動方向へキャラクターを向ける
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection, Vector3.up);

            Quaternion newRotation = Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                12.0f * Time.fixedDeltaTime
            );

            rb.MoveRotation(newRotation);
        }
    }

    private void JumpPlayer()
    {
        if (!jumpRequested)
        {
            return;
        }

        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x,
            0.0f,
            rb.linearVelocity.z
        );

        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        isGround = false;
        jumpRequested = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
}