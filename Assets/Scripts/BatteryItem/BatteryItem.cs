using UnityEngine;

public class BatteryItem : MonoBehaviour
{
    public float recoveryAmount = 30f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BatteryManager batteryManager = FindFirstObjectByType<BatteryManager>();

            if (batteryManager != null)
            {
                batteryManager.AddBattery(recoveryAmount);
            }

            Destroy(gameObject);
        }
    }
}
