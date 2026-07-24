using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class BatteryManager : MonoBehaviour
{
    [Header("Battery Settings")]
    public float maxBattery = 100f;
    public float currentBattery = 100f;
    public float drainSpeed = 1.0f;
    public bool flashlightOn = true;
    public Slider batterySlider;
    public TMP_Text batteryText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentBattery = maxBattery;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashlightOn)
        {
            DrainBattery(drainSpeed * Time.deltaTime);
        }

        batterySlider.value = currentBattery / maxBattery;

        batteryText.text = "BATTERY\n" + Mathf.CeilToInt(currentBattery) + "%";

        // Tキーで30減らす（テスト）
        //if (Keyboard.current.tKey.wasPressedThisFrame)
        //{
        //    DrainBattery(30f);
        //}

        //// Rキーで30回復（テスト）
        //if (Keyboard.current.rKey.wasPressedThisFrame)
        //{
        //    AddBattery(30f);
        //}
    }

    // バッテリーを減らす
    public void DrainBattery(float amount)
    {
        currentBattery -= amount;
        currentBattery = Mathf.Clamp(currentBattery, 0, maxBattery);
    }

    // バッテリーを回復する
    public void AddBattery(float amount)
    {
        currentBattery += amount;
        currentBattery = Mathf.Clamp(currentBattery, 0, maxBattery);
    }

    // 現在の残量を取得
    public float GetBattery()
    {
        return currentBattery;
    }

    // 最大値を取得
    public float GetMaxBattery()
    {
        return maxBattery;
    }

    // バッテリー切れかどうか
    public bool IsEmpty()
    {
        return currentBattery <= 0f;
    }
}
