using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class KonamiCodeInput : MonoBehaviour
{
    private enum CommandInput
    {
        Up,
        Down,
        Left,
        Right,
        B,
        A
    }

    [Header("ゲーム開始")]
    [SerializeField] private TitleInput titleInput;

    [Header("入力設定")]
    [SerializeField] private float inputResetTime = 2.0f;

    private readonly CommandInput[] konamiCode =
    {
        CommandInput.Up,
        CommandInput.Up,
        CommandInput.Down,
        CommandInput.Down,
        CommandInput.Left,
        CommandInput.Right,
        CommandInput.Left,
        CommandInput.Right,
        CommandInput.B,
        CommandInput.A
    };

    private int currentIndex;
    private float lastInputTime;
    private bool hasCompleted;

    private void Start()
    {
        // タイトルへ戻った時点でチートを解除
        CheatMode.Disable();
    }

    private void Update()
    {
        if (hasCompleted)
        {
            return;
        }

        if (currentIndex > 0 &&
            Time.unscaledTime >
            lastInputTime + inputResetTime)
        {
            ResetCommand();
        }

        if (!TryGetInput(out CommandInput input))
        {
            return;
        }

        lastInputTime = Time.unscaledTime;

        if (input == konamiCode[currentIndex])
        {
            currentIndex++;

            // 入力途中はタイトルメニューが動かないようにする
            if (currentIndex == 1)
            {
                SetMenuInputEnabled(false);
            }

            if (currentIndex >= konamiCode.Length)
            {
                CompleteCommand();
            }

            return;
        }

        // 間違えた入力が最初の「上」なら、1文字目として扱う
        if (input == konamiCode[0])
        {
            currentIndex = 1;
            SetMenuInputEnabled(false);
        }
        else
        {
            ResetCommand();
        }
    }

    private bool TryGetInput(
        out CommandInput commandInput)
    {
        // キーボード
        if (Keyboard.current != null)
        {
            if (Keyboard.current.upArrowKey
                .wasPressedThisFrame)
            {
                commandInput = CommandInput.Up;
                return true;
            }

            if (Keyboard.current.downArrowKey
                .wasPressedThisFrame)
            {
                commandInput = CommandInput.Down;
                return true;
            }

            if (Keyboard.current.leftArrowKey
                .wasPressedThisFrame)
            {
                commandInput = CommandInput.Left;
                return true;
            }

            if (Keyboard.current.rightArrowKey
                .wasPressedThisFrame)
            {
                commandInput = CommandInput.Right;
                return true;
            }

            if (Keyboard.current.bKey
                .wasPressedThisFrame)
            {
                commandInput = CommandInput.B;
                return true;
            }

            if (Keyboard.current.aKey
                .wasPressedThisFrame)
            {
                commandInput = CommandInput.A;
                return true;
            }
        }

        // Xboxコントローラー
        if (Gamepad.current != null)
        {
            if (Gamepad.current.dpad.up
                .wasPressedThisFrame)
            {
                commandInput = CommandInput.Up;
                return true;
            }

            if (Gamepad.current.dpad.down
                .wasPressedThisFrame)
            {
                commandInput = CommandInput.Down;
                return true;
            }

            if (Gamepad.current.dpad.left
                .wasPressedThisFrame)
            {
                commandInput = CommandInput.Left;
                return true;
            }

            if (Gamepad.current.dpad.right
                .wasPressedThisFrame)
            {
                commandInput = CommandInput.Right;
                return true;
            }

            // XboxのBボタン
            if (Gamepad.current.buttonEast
                .wasPressedThisFrame)
            {
                commandInput = CommandInput.B;
                return true;
            }

            // XboxのAボタン
            if (Gamepad.current.buttonSouth
                .wasPressedThisFrame)
            {
                commandInput = CommandInput.A;
                return true;
            }
        }

        commandInput = default;
        return false;
    }

    private void CompleteCommand()
    {
        hasCompleted = true;

        CheatMode.Enable();
        SetMenuInputEnabled(true);

        Debug.Log(
            "コナミコマンド成功：チートモードを有効化しました");

        // コマンド完成後、そのままゲームを開始
        if (titleInput != null)
        {
            titleInput.StartGame();
        }
        else
        {
            Debug.LogWarning(
                "KonamiCodeInput：TitleInputが設定されていません。");
        }
    }

    private void ResetCommand()
    {
        currentIndex = 0;
        SetMenuInputEnabled(true);
    }

    private void SetMenuInputEnabled(bool isEnabled)
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.sendNavigationEvents =
                isEnabled;
        }
    }

    private void OnDisable()
    {
        SetMenuInputEnabled(true);
    }
}