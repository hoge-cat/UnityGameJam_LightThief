using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialUIManager : MonoBehaviour
{
    public static TutorialUIManager Instance { get; private set; }

    private enum InputDeviceType
    {
        Keyboard,
        Gamepad
    }

    [Header("中央下のチュートリアル")]
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private CanvasGroup tutorialCanvasGroup;

    [Header("右下の操作一覧")]
    [SerializeField] private TextMeshProUGUI operationGuideText;
    [SerializeField] private CanvasGroup operationGuideCanvasGroup;

    [Header("フェード設定")]
    [SerializeField] private float fadeOutDuration = 0.6f;
    [SerializeField] private float fadeInDuration = 0.6f;

    private bool hasUsedFlashlight;
    private bool isFirstTutorialFading;
    private bool hasShownOperationGuide;

    private int nearbyDoorCount;
    private int nearbyTreasureCount;

    private bool currentDoorIsOpen;

    private InputDeviceType currentInputDevice =
        InputDeviceType.Keyboard;

    private Coroutine tutorialFadeCoroutine;
    private Coroutine operationGuideFadeCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (tutorialCanvasGroup != null)
        {
            tutorialCanvasGroup.alpha = 1.0f;
        }

        if (operationGuideCanvasGroup != null)
        {
            operationGuideCanvasGroup.alpha = 0.0f;
            operationGuideCanvasGroup.gameObject.SetActive(false);
        }

        RefreshText();
        RefreshOperationGuide();
    }

    private void Update()
    {
        DetectCurrentInputDevice();

        if (hasUsedFlashlight ||
            isFirstTutorialFading)
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
            StartFirstTutorialFade();
        }
    }

    private void DetectCurrentInputDevice()
    {
        bool keyboardUsed =
            Keyboard.current != null &&
            Keyboard.current.anyKey.wasPressedThisFrame;

        bool gamepadUsed =
            IsGamepadBeingUsed();

        if (gamepadUsed)
        {
            ChangeInputDevice(
                InputDeviceType.Gamepad);
        }
        else if (keyboardUsed)
        {
            ChangeInputDevice(
                InputDeviceType.Keyboard);
        }
    }

    private bool IsGamepadBeingUsed()
    {
        if (Gamepad.current == null)
        {
            return false;
        }

        Gamepad gamepad = Gamepad.current;

        bool buttonPressed =
            gamepad.buttonSouth.wasPressedThisFrame ||
            gamepad.buttonEast.wasPressedThisFrame ||
            gamepad.buttonWest.wasPressedThisFrame ||
            gamepad.buttonNorth.wasPressedThisFrame ||
            gamepad.leftShoulder.wasPressedThisFrame ||
            gamepad.rightShoulder.wasPressedThisFrame ||
            gamepad.leftStickButton.wasPressedThisFrame ||
            gamepad.rightStickButton.wasPressedThisFrame ||
            gamepad.startButton.wasPressedThisFrame ||
            gamepad.selectButton.wasPressedThisFrame ||
            gamepad.dpad.up.wasPressedThisFrame ||
            gamepad.dpad.down.wasPressedThisFrame ||
            gamepad.dpad.left.wasPressedThisFrame ||
            gamepad.dpad.right.wasPressedThisFrame;

        bool stickMoved =
            gamepad.leftStick.ReadValue().magnitude > 0.25f ||
            gamepad.rightStick.ReadValue().magnitude > 0.25f;

        return buttonPressed || stickMoved;
    }

    private void ChangeInputDevice(
        InputDeviceType newInputDevice)
    {
        if (currentInputDevice == newInputDevice)
        {
            return;
        }

        currentInputDevice = newInputDevice;

        RefreshOperationGuide();
        RefreshText();
    }

    private void StartFirstTutorialFade()
    {
        hasUsedFlashlight = true;
        isFirstTutorialFading = true;

        if (tutorialFadeCoroutine != null)
        {
            StopCoroutine(tutorialFadeCoroutine);
        }

        tutorialFadeCoroutine =
            StartCoroutine(
                FadeOutFirstTutorial());
    }

    private IEnumerator FadeOutFirstTutorial()
    {
        if (tutorialText == null)
        {
            isFirstTutorialFading = false;
            ShowOperationGuide();
            yield break;
        }

        if (tutorialCanvasGroup == null)
        {
            tutorialText.gameObject.SetActive(false);

            isFirstTutorialFading = false;
            ShowOperationGuide();
            yield break;
        }

        float startAlpha =
            tutorialCanvasGroup.alpha;

        float elapsedTime = 0.0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime +=
                Time.unscaledDeltaTime;

            float rate =
                Mathf.Clamp01(
                    elapsedTime /
                    fadeOutDuration);

            tutorialCanvasGroup.alpha =
                Mathf.Lerp(
                    startAlpha,
                    0.0f,
                    rate);

            yield return null;
        }

        tutorialCanvasGroup.alpha = 0.0f;

        // TutorialText自体は無効化しない
        tutorialText.gameObject.SetActive(true);

        isFirstTutorialFading = false;

        ShowOperationGuide();
        RefreshText();
    }

    private void ShowOperationGuide()
    {
        if (hasShownOperationGuide)
        {
            return;
        }

        hasShownOperationGuide = true;

        RefreshOperationGuide();

        if (operationGuideText == null)
        {
            return;
        }

        operationGuideText.gameObject.SetActive(true);

        if (operationGuideCanvasGroup == null)
        {
            return;
        }

        operationGuideCanvasGroup.gameObject.SetActive(true);
        operationGuideCanvasGroup.alpha = 0.0f;

        if (operationGuideFadeCoroutine != null)
        {
            StopCoroutine(
                operationGuideFadeCoroutine);
        }

        operationGuideFadeCoroutine =
            StartCoroutine(
                FadeInOperationGuide());
    }

    private IEnumerator FadeInOperationGuide()
    {
        if (operationGuideCanvasGroup == null)
        {
            yield break;
        }

        float elapsedTime = 0.0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime +=
                Time.unscaledDeltaTime;

            float rate =
                Mathf.Clamp01(
                    elapsedTime /
                    fadeInDuration);

            operationGuideCanvasGroup.alpha = rate;

            yield return null;
        }

        operationGuideCanvasGroup.alpha = 1.0f;
    }

    private void RefreshOperationGuide()
    {
        if (operationGuideText == null)
        {
            return;
        }

        if (currentInputDevice ==
            InputDeviceType.Gamepad)
        {
            operationGuideText.text =
                "X: ダッシュ\n" +
                "A: ジャンプ\n" +
                "B: 調べる・拾う\n" +
                "RB: ライト";
        }
        else
        {
            operationGuideText.text =
                "Shift: ダッシュ\n" +
                "Space: ジャンプ\n" +
                "E: 調べる・拾う\n" +
                "F: ライト";
        }
    }

    public void ShowDoorPrompt(bool isOpen)
    {
        Debug.Log("ShowDoorPrompt");

        nearbyDoorCount++;
        SetDoorPrompt(isOpen);
    }

    public void HideDoorPrompt()
    {
        nearbyDoorCount =
            Mathf.Max(
                0,
                nearbyDoorCount - 1);

        RefreshText();
    }

    public void SetDoorPrompt(bool isOpen)
    {
        currentDoorIsOpen = isOpen;
        RefreshText();
    }

    public void ShowTreasurePrompt()
    {
        nearbyTreasureCount++;
        RefreshText();
    }

    public void HideTreasurePrompt()
    {
        nearbyTreasureCount =
            Mathf.Max(
                0,
                nearbyTreasureCount - 1);

        RefreshText();
    }

    private void RefreshText()
    {
        if (tutorialText == null ||
            isFirstTutorialFading)
        {
            return;
        }

        if (nearbyTreasureCount > 0)
        {
            ShowTutorialText(
                currentInputDevice ==
                InputDeviceType.Gamepad
                    ? "[ B ]  宝を取得"
                    : "[ E ]  宝を取得");

            return;
        }

        if (nearbyDoorCount > 0)
        {
            string actionText =
                currentDoorIsOpen
                    ? "閉じる"
                    : "開ける";

            string prompt =
                currentInputDevice ==
                InputDeviceType.Gamepad
                    ? $"[ B ]  ドアを{actionText}"
                    : $"[ E ]  ドアを{actionText}";

            ShowTutorialText(prompt);
            return;
        }

        if (!hasUsedFlashlight)
        {
            string prompt =
                currentInputDevice ==
                InputDeviceType.Gamepad
                    ? "RB: ライト点灯"
                    : "F: ライト点灯";

            ShowTutorialText(prompt);
            return;
        }

        HideTutorialTextImmediately();
    }

    private void ShowTutorialText(string message)
    {
        if (tutorialText == null)
        {
            return;
        }

        tutorialText.gameObject.SetActive(true);
        tutorialText.enabled = true;
        tutorialText.text = message;

        Color textColor = tutorialText.color;
        textColor.a = 1.0f;
        tutorialText.color = textColor;

        if (tutorialCanvasGroup != null)
        {
            tutorialCanvasGroup.alpha = 1.0f;
        }
    }

    private void HideTutorialTextImmediately()
    {
        if (tutorialText == null)
        {
            return;
        }

        // オブジェクトは無効化せず透明にする
        tutorialText.gameObject.SetActive(true);

        if (tutorialCanvasGroup != null)
        {
            tutorialCanvasGroup.alpha = 0.0f;
        }
    }
}