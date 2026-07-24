using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour
{
    [Header("開閉設定")]
    [SerializeField] private float openAngle = 90.0f;
    [SerializeField] private float openSpeed = 2.0f;

    [Header("操作設定")]
    [SerializeField] private float interactionDistance = 2.0f;

    private Transform player;

    private bool isOpen;
    private Quaternion closedRotation;
    private Quaternion openedRotation;

    private void Start()
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
                "DoorController：Playerタグのオブジェクトが見つかりません。");
        }

        closedRotation = transform.localRotation;

        openedRotation =
            closedRotation *
            Quaternion.Euler(0.0f, openAngle, 0.0f);
    }

    private void Update()
    {
        ReadInput();
        UpdateDoorRotation();
    }

    private void ReadInput()
    {
        if (player == null)
        {
            return;
        }

        float distanceToPlayer =
            Vector3.Distance(
                player.position,
                transform.position);

        if (distanceToPlayer > interactionDistance)
        {
            return;
        }

        bool keyboardPressed =
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame;

        bool gamepadPressed =
            Gamepad.current != null &&
            Gamepad.current.buttonNorth.wasPressedThisFrame;

        if (keyboardPressed || gamepadPressed)
        {
            isOpen = !isOpen;
        }
    }

    private void UpdateDoorRotation()
    {
        Quaternion targetRotation =
            isOpen ? openedRotation : closedRotation;

        transform.localRotation =
            Quaternion.Slerp(
                transform.localRotation,
                targetRotation,
                openSpeed * Time.deltaTime);
    }
}