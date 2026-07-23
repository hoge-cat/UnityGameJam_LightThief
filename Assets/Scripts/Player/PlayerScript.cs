using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    enum Direction
    {
        None,
        Right,
        Left,
        Front,
        Back
    }

    Vector3 startPosition;
    float moveSpeed;
    float dashSpeed;
    Rigidbody rb;
    float jumpPower;
    bool isGround = true;
    Camera mainCamera;
    //float limitDistance;
    Direction direction = Direction.None;
    //int myStatus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //direction = Direction.None;
        InitMan();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Keyboard.current);

        MoveMan();
    }

    void InitMan()
    {
        rb = GetComponent<Rigidbody>();

        moveSpeed = 2.5f;
        dashSpeed = 5.0f;
        jumpPower = 5.0f;
        //limitDistance = 7.5f;
        startPosition = transform.position;

        mainCamera = Camera.main;
    }

    void MoveMan()
    {
        //Debug.Log("Move");
        //Debug.Log(transform.position);
        //Debug.Log("MoveMan");

        float speed = moveSpeed;

        direction = Direction.None;

        Vector3 moveDirection = Vector3.zero;

        if ((Keyboard.current.kKey.isPressed) ||
     (Gamepad.current != null && Gamepad.current.buttonWest.isPressed))
        {
            speed = dashSpeed;
        }

        //if (transform.position.x >= limitDistance)
        //{
        //    transform.position = startPosition;
        //}

        else if (Keyboard.current.wKey.isPressed)
        {
            direction = Direction.Front;
        }

        else if (Keyboard.current.sKey.isPressed)
        {
            direction = Direction.Back;
        }

        else if (Keyboard.current.aKey.isPressed)
        {
            direction = Direction.Left;
        }

        else if (Keyboard.current.dKey.isPressed)
        {
            direction = Direction.Right;
        }

        else if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.leftStick.ReadValue();

            if (stick.y > 0.5f)
            {
                direction = Direction.Front;
            }
            else if (stick.y < -0.5f)
            {
                direction = Direction.Back;
            }
            else if (stick.x < -0.5f)
            {
                direction = Direction.Left;
            }
            else if (stick.x > 0.5f)
            {
                direction = Direction.Right;
            }

        else
            {
                return;
            }
        }

        else
        {
            return;
        }

        Vector3 move = Vector3.zero;

        Transform cam = mainCamera.transform;

        // カメラの前方向・右方向
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        // 上下方向を無視
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        switch (direction)
        {
            case Direction.Front:
                move = forward;
                break;

            case Direction.Back:
                move = -forward;
                break;

            case Direction.Left:
                move = -right;
                break;

            case Direction.Right:
                move = right;
                break;
        }

        transform.position += move * speed * Time.deltaTime;



        if (isGround &&
            ((Keyboard.current.jKey.wasPressedThisFrame) ||
             (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGround = false;
        }
    }

    void SetMoveSpeed(float inMoveSpeed)
    {
        moveSpeed *= inMoveSpeed;
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
