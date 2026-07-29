using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour
{
    [Header("開閉設定")]
    [SerializeField] private float openAngle = 90.0f;
    [SerializeField] private float openSpeed = 2.0f;

    [Header("操作設定")]
    [SerializeField] private float interactionDistance = 2.0f;

    [Header("効果音")]
    [SerializeField] private DoorSound doorSound;

    private Transform player;

    private bool isOpen;
    private bool wasPlayerInRange;

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
        UpdateInteraction();
        UpdateDoorRotation();
    }

    private void UpdateInteraction()
    {
        if (player == null)
        {
            return;
        }

        Vector3 playerPosition = player.position;
        Vector3 doorPosition = transform.position;

        // 高さの違いを無視して、床の上での距離だけを測る
        playerPosition.y = 0.0f;
        doorPosition.y = 0.0f;

        float distanceToPlayer =
            Vector3.Distance(
                playerPosition,
                doorPosition);

        bool isPlayerInRange =
            distanceToPlayer <= interactionDistance;

        // ドアの範囲に入った瞬間
        if (isPlayerInRange && !wasPlayerInRange)
        {
            Debug.Log("Door Prompt");

            TutorialUIManager.Instance?.ShowDoorPrompt(isOpen);
        }

        // ドアの範囲から出た瞬間
        if (!isPlayerInRange && wasPlayerInRange)
        {
            TutorialUIManager.Instance?.HideDoorPrompt();
        }

        wasPlayerInRange = isPlayerInRange;

        if (!isPlayerInRange)
        {
            return;
        }

        bool keyboardPressed =
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame;

        bool gamepadPressed =
    Gamepad.current != null &&
    Gamepad.current.buttonEast.wasPressedThisFrame;

        if (keyboardPressed || gamepadPressed)
        {
            PlayerScript playerScript =
                player.GetComponent<PlayerScript>();

            if (playerScript != null)
            {
                playerScript.PlayDoorAnimation();
            }

            isOpen = !isOpen;

            if (doorSound != null)
            {
                if (isOpen)
                {
                    doorSound.PlayOpenSound();
                }
                else
                {
                    doorSound.PlayCloseSound();
                }
            }

            TutorialUIManager.Instance?.SetDoorPrompt(isOpen);
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

    private void OnDisable()
    {
        if (wasPlayerInRange)
        {
            TutorialUIManager.Instance?.HideDoorPrompt();
            wasPlayerInRange = false;
        }
    }
}