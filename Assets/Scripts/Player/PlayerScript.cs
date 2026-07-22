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
    float limitDistance;
    Direction direction = Direction.None;
    //int myStatus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = Direction.None;
        InitMan();
    }

    // Update is called once per frame
    void Update()
    {
        MoveMan();
    }

    void InitMan()
    {
        moveSpeed = 2.5f;
        limitDistance = 7.5f;
    }

    void MoveMan()
    {
        Vector3 moveDirection = Vector3.zero;

        //transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        if (transform.position.x >= limitDistance)
        {
            transform.position = startPosition;
        }

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

        switch (direction)
        {
            case Direction.None:
                break;

            case Direction.Front:
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                break;

            case Direction.Back:
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
                break;

            case Direction.Left:
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                break;

            case Direction.Right:
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                break;
        }
    }

    void SetMoveSpeed(float inMoveSpeed)
    {
        moveSpeed *= inMoveSpeed;
    }
}
