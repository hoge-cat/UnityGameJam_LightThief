using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [Header("ライト設定")]
    [SerializeField] private Light flashlight;

    [SerializeField] private BatteryManager batteryManager;

    [Header("操作設定")]
    [SerializeField] private bool startWithLightOn = false;

    private bool isLightOn;

    private void Awake()
    {
        if (flashlight == null)
        {
            flashlight =
                GetComponentInChildren<Light>();
        }

        // コナミコマンド成功時は明るさを3倍
        if (CheatMode.IsEnabled &&
            flashlight != null)
        {
            flashlight.intensity *= 3.0f;

            Debug.Log("チート：懐中電灯の明るさ3倍");
        }

        isLightOn = startWithLightOn;
        ApplyLightState();

        if (batteryManager != null)
        {
            batteryManager.SetFlashlightState(
                isLightOn);
        }
    }

    private void Update()
    {
        // バッテリー切れなら自動でライトを消す
        if (isLightOn &&
            batteryManager != null &&
            batteryManager.IsEmpty())
        {
            isLightOn = false;
            ApplyLightState();
            batteryManager.SetFlashlightState(false);
        }

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
        // ライトOFF→ONにするとき、バッテリーが無ければ点灯しない
        if (!isLightOn &&
            batteryManager != null &&
            !batteryManager.CanUseFlashlight())
        {
            return;
        }

        isLightOn = !isLightOn;

        ApplyLightState();

        if (batteryManager != null)
        {
            batteryManager.SetFlashlightState(isLightOn);
        }
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