using UnityEngine;

public class SecurityCameraRotation : MonoBehaviour
{
    [Header("首振り設定")]
    [SerializeField] private float rotationAngle = 60.0f;
    [SerializeField] private float rotationSpeed = 30.0f;

    private float startRotationY;
    private float currentAngle;
    private int rotationDirection = 1;

    private void Start()
    {
        startRotationY = transform.localEulerAngles.y;
    }

    private void Update()
    {
        currentAngle += rotationSpeed * rotationDirection * Time.deltaTime;

        float halfAngle = rotationAngle * 0.5f;

        if (currentAngle >= halfAngle)
        {
            currentAngle = halfAngle;
            rotationDirection = -1;
        }
        else if (currentAngle <= -halfAngle)
        {
            currentAngle = -halfAngle;
            rotationDirection = 1;
        }

        transform.localRotation =
            Quaternion.Euler(0.0f, startRotationY + currentAngle, 0.0f);
    }
}