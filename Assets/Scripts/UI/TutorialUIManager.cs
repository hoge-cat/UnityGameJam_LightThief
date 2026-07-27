using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialUIManager : MonoBehaviour
{
    public static TutorialUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI tutorialText;

    private bool hasUsedFlashlight;
    private int nearbyDoorCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshText();
    }

    private void Update()
    {
        if (hasUsedFlashlight)
        {
            return;
        }

        bool keyboardPressed =
            Keyboard.current != null &&
            Keyboard.current.fKey.wasPressedThisFrame;

        bool gamepadPressed =
            Gamepad.current != null &&
            Gamepad.current.rightShoulder.wasPressedThisFrame;

        if (keyboardPressed || gamepadPressed)
        {
            hasUsedFlashlight = true;
            RefreshText();
        }
    }

    public void ShowDoorPrompt(bool isOpen)
    {
        nearbyDoorCount++;
        SetDoorPrompt(isOpen);
    }

    public void HideDoorPrompt()
    {
        nearbyDoorCount =
            Mathf.Max(0, nearbyDoorCount - 1);

        RefreshText();
    }

    public void SetDoorPrompt(bool isOpen)
    {
        if (tutorialText == null)
        {
            return;
        }

        tutorialText.text =
            isOpen ? "Eキー: 閉じる" : "Eキー: 開ける";

        tutorialText.gameObject.SetActive(true);
    }

    private void RefreshText()
    {
        if (tutorialText == null)
        {
            return;
        }

        // ドアの近くではドア操作を優先
        if (nearbyDoorCount > 0)
        {
            tutorialText.text = "Eキー: 開ける";
            tutorialText.gameObject.SetActive(true);
            return;
        }

        // 初めてライトを点灯するまでは表示
        if (!hasUsedFlashlight)
        {
            tutorialText.text = "Fキー: ライト点灯";
            tutorialText.gameObject.SetActive(true);
            return;
        }

        tutorialText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}