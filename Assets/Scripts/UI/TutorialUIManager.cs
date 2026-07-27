using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialUIManager : MonoBehaviour
{
    public static TutorialUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI tutorialText;

    private bool hasUsedFlashlight;
    private int nearbyDoorCount;
    private int nearbyTreasureCount;

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

        if (nearbyTreasureCount > 0)
        {
            tutorialText.text = "Eキー: 取得";
            tutorialText.gameObject.SetActive(true);
            return;
        }

        if (nearbyDoorCount > 0)
        {
            tutorialText.gameObject.SetActive(true);
            return;
        }

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

    public void ShowTreasurePrompt()
    {
        nearbyTreasureCount++;
        RefreshText();
    }

    public void HideTreasurePrompt()
    {
        nearbyTreasureCount =
            Mathf.Max(0, nearbyTreasureCount - 1);

        RefreshText();
    }
}