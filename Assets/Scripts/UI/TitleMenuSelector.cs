using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitleMenuSelector : MonoBehaviour
{
    [Header("最初の選択")]
    [SerializeField] private Button firstSelectedButton;

    [Header("メニュー音")]
    [SerializeField] private TitleMenuSounds menuSounds;

    [Header("選択するボタン")]
    [SerializeField] private List<Button> menuButtons;

    private int currentIndex = 0;

    private GameObject previousSelectedObject;

    private void Start()
    {
        currentIndex = 0;

        SelectFirstButton();

        if (EventSystem.current != null)
        {
            previousSelectedObject =
                EventSystem.current.currentSelectedGameObject;
        }
    }

    private void Update()
    {
        RestoreSelection();

        if (Keyboard.current != null)
        {
            if (Keyboard.current.sKey.wasPressedThisFrame ||
                Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                ChangeSelection(1);
            }

            if (Keyboard.current.wKey.wasPressedThisFrame ||
                Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                ChangeSelection(-1);
            }
        }
    }

    private void RestoreSelection()
    {
        bool keyboardInput =
            Keyboard.current != null &&
            (
                Keyboard.current.upArrowKey.wasPressedThisFrame ||
                Keyboard.current.downArrowKey.wasPressedThisFrame ||
                Keyboard.current.wKey.wasPressedThisFrame ||
                Keyboard.current.sKey.wasPressedThisFrame
            );

        bool gamepadInput =
            Gamepad.current != null &&
            (
                Gamepad.current.dpad.up.wasPressedThisFrame ||
                Gamepad.current.dpad.down.wasPressedThisFrame ||
                Gamepad.current.leftStick.up.wasPressedThisFrame ||
                Gamepad.current.leftStick.down.wasPressedThisFrame
            );

        if (!keyboardInput && !gamepadInput)
        {
            return;
        }

        if (EventSystem.current == null)
        {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            SelectFirstButton();
        }
    }

    public void SelectFirstButton()
    {
        if (firstSelectedButton == null ||
            EventSystem.current == null)
        {
            return;
        }

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(
            firstSelectedButton.gameObject
        );
    }

    private void ChangeSelection(int direction)
    {
        if (menuButtons == null || menuButtons.Count == 0)
        {
            return;
        }

        currentIndex += direction;

        if (currentIndex < 0)
        {
            currentIndex = menuButtons.Count - 1;
        }

        if (currentIndex >= menuButtons.Count)
        {
            currentIndex = 0;
        }

        if (EventSystem.current == null)
        {
            return;
        }

        EventSystem.current.SetSelectedGameObject(
            menuButtons[currentIndex].gameObject
        );

        menuSounds?.PlayMove();
    }
}