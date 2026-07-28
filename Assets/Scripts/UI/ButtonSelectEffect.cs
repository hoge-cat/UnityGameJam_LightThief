using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectEffect : MonoBehaviour
{
    [Header("表示設定")]
    [SerializeField] private float selectedScale = 1.08f;
    [SerializeField] private float changeSpeed = 10.0f;

    private Vector3 normalScale;
    private Vector3 targetScale;

    private void Awake()
    {
        normalScale = transform.localScale;
        targetScale = normalScale;
    }

    private void Update()
    {
        bool isSelected =
            EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject ==
            gameObject;

        targetScale =
            isSelected
                ? normalScale * selectedScale
                : normalScale;

        transform.localScale =
            Vector3.Lerp(
                transform.localScale,
                targetScale,
                changeSpeed * Time.unscaledDeltaTime
            );
    }

    private void OnDisable()
    {
        transform.localScale = normalScale;
    }
}