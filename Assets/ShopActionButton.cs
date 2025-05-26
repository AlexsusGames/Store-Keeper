using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopActionButton : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;

    [SerializeField] private Color standartColor;
    [SerializeField] private Color rentColor;

    private string standartText = "Rent";
    private string rentText = "Supply";

    private Button button;

    public void Hide() => AssignListener(null);

    public void SetDeliveryAction(UnityAction action)
    {
        AssignListener(action);

        button.image.color = rentColor;
        buttonText.text = Core.Localization.Translate(rentText);
    }

    public void SetRentAction(UnityAction action)
    {
        AssignListener(action);

        button.image.color = standartColor;
        buttonText.text = Core.Localization.Translate(standartText);
    }

    private void AssignListener(UnityAction action)
    {
        if (button == null) button = GetComponent<Button>();
        if (action == null) button.gameObject.SetActive(false);
        else button.gameObject.SetActive(true);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
}
