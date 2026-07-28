using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResultInput : MonoBehaviour
{
    [Header("シーン設定")]
    [SerializeField] private string mainSceneName = "Main";
    [SerializeField] private string titleSceneName = "Title";

    [Header("メニュー表示")]
    [SerializeField] private TMP_Text retryText;
    [SerializeField] private TMP_Text titleText;

    [Header("入力設定")]
    [SerializeField] private float inputDelay = 0.5f;
    [SerializeField] private float repeatDelay = 0.2f;

    [Header("効果音")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cursorMoveSound;
    [SerializeField] private AudioClip submitSound;

    private int selectedIndex;
    private float elapsedTime;
    private float lastMoveTime;
    private bool isLoading;

    private void Start()
    {
        Time.timeScale = 1.0f;
        UpdateMenuDisplay();
    }

    private void Update()
    {
        if (isLoading)
        {
            return;
        }

        elapsedTime += Time.unscaledDeltaTime;

        if (elapsedTime < inputDelay)
        {
            return;
        }

        ReadSelectionInput();
        ReadSubmitInput();
    }

    private void ReadSelectionInput()
    {
        bool moveUp = false;
        bool moveDown = false;

        if (Keyboard.current != null)
        {
            moveUp =
                Keyboard.current.wKey.wasPressedThisFrame ||
                Keyboard.current.upArrowKey.wasPressedThisFrame;

            moveDown =
                Keyboard.current.sKey.wasPressedThisFrame ||
                Keyboard.current.downArrowKey.wasPressedThisFrame;
        }

        if (Gamepad.current != null &&
            Time.unscaledTime >= lastMoveTime + repeatDelay)
        {
            float stickY =
                Gamepad.current.leftStick.ReadValue().y;

            moveUp |=
                Gamepad.current.dpad.up.wasPressedThisFrame ||
                stickY > 0.5f;

            moveDown |=
                Gamepad.current.dpad.down.wasPressedThisFrame ||
                stickY < -0.5f;

            if (moveUp || moveDown)
            {
                lastMoveTime = Time.unscaledTime;
            }
        }

        if (moveUp)
        {
            if (selectedIndex != 0)
            {
                selectedIndex = 0;
                PlaySound(cursorMoveSound);
                UpdateMenuDisplay();
            }
        }
        else if (moveDown)
        {
            if (selectedIndex != 1)
            {
                selectedIndex = 1;
                PlaySound(cursorMoveSound);
                UpdateMenuDisplay();
            }
        }
    }

    private void ReadSubmitInput()
    {
        bool keyboardSubmit =
            Keyboard.current != null &&
            (
                Keyboard.current.enterKey.wasPressedThisFrame ||
                Keyboard.current.numpadEnterKey.wasPressedThisFrame ||
                Keyboard.current.spaceKey.wasPressedThisFrame
            );

        bool gamepadSubmit =
            Gamepad.current != null &&
            Gamepad.current.buttonSouth.wasPressedThisFrame;

        if (!keyboardSubmit && !gamepadSubmit)
        {
            return;
        }

        isLoading = true;

        PlaySound(submitSound);

        Time.timeScale = 1.0f;

        if (selectedIndex == 0)
        {
            SceneManager.LoadScene(mainSceneName);
        }
        else
        {
            SceneManager.LoadScene(titleSceneName);
        }
    }

    private void UpdateMenuDisplay()
    {
        Color selectedColor = Color.yellow;
        Color normalColor = Color.white;

        if (retryText != null)
        {
            retryText.text =
                selectedIndex == 0
                ? "> もう一度"
                : "  もう一度";

            retryText.color =
                selectedIndex == 0
                ? selectedColor
                : normalColor;
        }

        if (titleText != null)
        {
            titleText.text =
                selectedIndex == 1
                ? "> タイトルへ戻る"
                : "  タイトルへ戻る";

            titleText.color =
                selectedIndex == 1
                ? selectedColor
                : normalColor;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
    }
}