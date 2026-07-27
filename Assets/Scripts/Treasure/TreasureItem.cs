using UnityEngine;
using UnityEngine.InputSystem;

public class TreasureItem : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2.0f;

    private Transform player;
    private bool wasPlayerInRange;
    private bool hasCollected;

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
                "TreasureItem：Playerタグのオブジェクトが見つかりません。");
        }
    }

    private void Update()
    {
        if (hasCollected || player == null)
        {
            return;
        }

        float distanceToPlayer =
            Vector3.Distance(
                player.position,
                transform.position);

        bool isPlayerInRange =
            distanceToPlayer <= interactionDistance;

        if (isPlayerInRange && !wasPlayerInRange)
        {
            TutorialUIManager.Instance?.ShowTreasurePrompt();
        }

        if (!isPlayerInRange && wasPlayerInRange)
        {
            TutorialUIManager.Instance?.HideTreasurePrompt();
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
            Gamepad.current.buttonNorth.wasPressedThisFrame;

        if (keyboardPressed || gamepadPressed)
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (hasCollected)
        {
            return;
        }

        hasCollected = true;

        if (wasPlayerInRange)
        {
            TutorialUIManager.Instance?.HideTreasurePrompt();
            wasPlayerInRange = false;
        }

        TreasureManager.Instance?.CollectTreasure();

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (wasPlayerInRange)
        {
            TutorialUIManager.Instance?.HideTreasurePrompt();
            wasPlayerInRange = false;
        }
    }
}