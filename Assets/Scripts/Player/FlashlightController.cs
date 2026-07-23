using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [Header("ライト設定")]
    [SerializeField] private Light flashlight;

    [Header("操作設定")]
    [SerializeField] private bool startWithLightOn = true;

    private bool isLightOn;

    private void Awake()
    {
        if (flashlight == null)
        {
            flashlight = GetComponentInChildren<Light>();
        }

        isLightOn = startWithLightOn;
        ApplyLightState();
    }

    private void Update()
    {
        bool keyboardPressed =
            Keyboard.current != null &&
            Keyboard.current.fKey.wasPressedThisFrame;

        bool gamepadPressed =
            Gamepad.current != null &&
            Gamepad.current.rightShoulder.wasPressedThisFrame;

        if (keyboardPressed || gamepadPressed)
        {
            ToggleFlashlight();
        }
    }

    public void ToggleFlashlight()
    {
        isLightOn = !isLightOn;
        ApplyLightState();
    }

    private void ApplyLightState()
    {
        if (flashlight != null)
        {
            flashlight.enabled = isLightOn;
        }
    }

    public bool IsLightOn()
    {
        return isLightOn;
    }
}