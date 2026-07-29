using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TreasureItem : MonoBehaviour
{
    [Header("操作設定")]
    [SerializeField] private float interactionDistance = 2.0f;

    [Header("取得演出")]
    [SerializeField] private float collectDelay = 0.8f;

    private Transform player;
    private Animator playerAnimator;

    private bool wasPlayerInRange;
    private bool hasCollected;

    private static readonly int PickUpHash =
        Animator.StringToHash("PickUp");

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerAnimator =
                playerObject.GetComponentInChildren<Animator>();
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

        if (isPlayerInRange)
        {
            if (!wasPlayerInRange)
            {
                TutorialUIManager.Instance?.ShowTreasurePrompt();
            }

            TutorialUIManager.Instance?.RefreshInteractionPrompt();
        }
        else if (wasPlayerInRange)
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
            Gamepad.current.buttonEast.wasPressedThisFrame;

        if (keyboardPressed || gamepadPressed)
        {
            StartCollect();
        }
    }

    private void StartCollect()
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

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger(PickUpHash);
        }

        StartCoroutine(CollectAfterAnimation());
    }

    private IEnumerator CollectAfterAnimation()
    {
        yield return new WaitForSeconds(collectDelay);

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