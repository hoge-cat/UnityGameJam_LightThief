using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleMenuSelector : MonoBehaviour
{
    [Header("ç≈èâÇÃëIë")]
    [SerializeField] private Button firstSelectedButton;

    [Header("ÉÅÉjÉÖÅ[âπ")]
    [SerializeField] private TitleMenuSounds menuSounds;

    private GameObject previousSelectedObject;

    private void Start()
    {
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
        ReadSubmitInput();

        if (EventSystem.current == null)
        {
            return;
        }

        GameObject currentSelectedObject =
            EventSystem.current.currentSelectedGameObject;

        if (currentSelectedObject != null &&
            currentSelectedObject != previousSelectedObject)
        {
            menuSounds?.PlayMove();
            previousSelectedObject = currentSelectedObject;
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

    private void ReadSubmitInput()
    {
        bool keyboardSubmit =
            Keyboard.current != null &&
            (
                Keyboard.current.spaceKey.wasPressedThisFrame ||
                Keyboard.current.enterKey.wasPressedThisFrame ||
                Keyboard.current.numpadEnterKey.wasPressedThisFrame
            );

        if (!keyboardSubmit)
        {
            return;
        }

        if (EventSystem.current == null)
        {
            return;
        }

        GameObject selectedObject =
            EventSystem.current.currentSelectedGameObject;

        if (selectedObject == null)
        {
            SelectFirstButton();

            selectedObject =
                EventSystem.current.currentSelectedGameObject;
        }

        if (selectedObject == null)
        {
            return;
        }

        Button selectedButton =
            selectedObject.GetComponent<Button>();

        if (selectedButton == null)
        {
            selectedButton =
                selectedObject.GetComponentInParent<Button>();
        }

        if (selectedButton != null &&
            selectedButton.interactable)
        {
            selectedButton.onClick.Invoke();
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
}