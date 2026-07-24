using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleInput : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "Main";

    [Header("誤入力防止")]
    [SerializeField] private float inputDelay = 0.5f;

    private float elapsedTime;
    private bool isLoading;

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

        bool keyboardPressed =
            Keyboard.current != null &&
            Keyboard.current.anyKey.wasPressedThisFrame;

        bool gamepadPressed =
            Gamepad.current != null &&
            (
                Gamepad.current.buttonSouth.wasPressedThisFrame ||
                Gamepad.current.buttonEast.wasPressedThisFrame ||
                Gamepad.current.buttonWest.wasPressedThisFrame ||
                Gamepad.current.buttonNorth.wasPressedThisFrame ||
                Gamepad.current.startButton.wasPressedThisFrame
            );

        if (keyboardPressed || gamepadPressed)
        {
            isLoading = true;
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(mainSceneName);
        }
    }
}