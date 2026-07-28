using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleMenuSelector : MonoBehaviour
{
    [SerializeField] private Button firstSelectedButton;

    private void Start()
    {
        SelectFirstButton();
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