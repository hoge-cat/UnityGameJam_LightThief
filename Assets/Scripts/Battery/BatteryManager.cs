using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BatteryManager : MonoBehaviour
{
    [Header("バッテリー設定")]
    [SerializeField] private float maxBattery = 100.0f;
    [SerializeField] private float currentBattery = 100.0f;
    [SerializeField] private float drainSpeed = 1.0f;

    [Header("UI")]
    [SerializeField] private Slider batterySlider;
    [SerializeField] private TMP_Text batteryText;

    private bool flashlightOn;

    private void Start()
    {
        currentBattery = maxBattery;
        flashlightOn = false;

        UpdateUI();
    }

    private void Update()
    {
        // チート中は常に満タン
        if (CheatMode.IsEnabled)
        {
            currentBattery = maxBattery;
            UpdateUI();
            return;
        }

        if (flashlightOn && !IsEmpty())
        {
            DrainBattery(
                drainSpeed * Time.deltaTime);
        }

        if (IsEmpty())
        {
            flashlightOn = false;

            if (!GoalTrigger.IsClearing)
            {
                SceneManager.LoadScene(
                    "GameOver");
            }

            return;
        }

        UpdateUI();
    }

    public void SetFlashlightState(bool isOn)
    {
        if (IsEmpty())
        {
            flashlightOn = false;
            return;
        }

        flashlightOn = isOn;
    }

    public bool CanUseFlashlight()
    {
        return !IsEmpty();
    }

    public void DrainBattery(float amount)
    {
        if (CheatMode.IsEnabled)
        {
            currentBattery = maxBattery;
            return;
        }

        currentBattery -= amount;

        currentBattery = Mathf.Clamp(
            currentBattery,
            0.0f,
            maxBattery
        );
    }

    public void AddBattery(float amount)
    {
        currentBattery += amount;

        currentBattery = Mathf.Clamp(
            currentBattery,
            0.0f,
            maxBattery
        );

        UpdateUI();
    }

    public float GetBattery()
    {
        return currentBattery;
    }

    public float GetMaxBattery()
    {
        return maxBattery;
    }

    public bool IsEmpty()
    {
        if (CheatMode.IsEnabled)
        {
            return false;
        }

        return currentBattery <= 0.0f;
    }

    private void UpdateUI()
    {
        if (batterySlider != null)
        {
            batterySlider.value =
                currentBattery / maxBattery;
        }

        if (batteryText != null)
        {
            batteryText.text =
                "BATTERY\n" +
                Mathf.CeilToInt(
                    currentBattery
                ) +
                "%";
        }
    }
}