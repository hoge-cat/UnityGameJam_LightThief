using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;

    private bool waitingForFirstLightInput = true;

    private void Start()
    {
        if (tutorialText == null)
        {
            return;
        }

        tutorialText.text = "Fキー: ライト点灯";
        tutorialText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!waitingForFirstLightInput)
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
            waitingForFirstLightInput = false;

            if (tutorialText != null)
            {
                tutorialText.gameObject.SetActive(false);
            }
        }
    }
}