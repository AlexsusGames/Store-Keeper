using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SupplyPresenter : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text companyName;

    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonText;

    [SerializeField] private Color greenColor;
    [SerializeField] private Color redColor;

    private readonly string[] companyNames = { "LLC \"MarketWay Distributors\"", "Inc. \"Bakery\"", "Corp. \"Dairy\"" };

    public void AssignListener(UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    public void IsSupplied(bool value)
    {
        Color color = value ? redColor : greenColor;
        string action = value ? "Finish" : "Supply";

        SetButtonView(color, action);
    }

    private void SetButtonView(Color color, string actionText)
    {
        buttonText.text = actionText;
        button.image.color = color;
    }
    public void SetData(CarType carType, Sprite sprite, UnityAction action)
    {
        Hide();
        gameObject.SetActive(true);

        image.sprite = sprite;
        companyName.text = companyNames[(int)carType];

        AssignListener(action);
    }

    public void Hide() => gameObject.SetActive(false);
}
