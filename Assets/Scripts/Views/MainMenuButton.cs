using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text text;

    private void Awake() => text = GetComponent<TMP_Text>();

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Underline;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Normal;
    }
}
