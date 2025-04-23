using System;
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
    [SerializeField] private TMP_Text cost;

    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonText;

    [SerializeField] private Color greenColor;
    [SerializeField] private Color redColor;

    private readonly string[] companyNames = { "LLC 'MarketWay Distributors'", "Inc. 'Bakery'", "Corp. 'Dairy'", "Inc. 'Brillex'", "Corp. 'Nordwell'" };

    public void AssignListener(UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    public void IsSupplied(bool value)
    {
        Color color = value ? redColor : greenColor;
        string action = value ? "Complete" : "Supply";

        string translated = Core.Localization.Translate(action);

        cost.gameObject.SetActive(!value);

        SetButtonView(color, translated);
    }

    private void SetButtonView(Color color, string actionText)
    {
        buttonText.text = actionText;
        button.image.color = color;
    }
    public void SetData(CarType carType, Sprite sprite, UnityAction action, float price)
    {
        Hide();
        gameObject.SetActive(true);

        IsSupplied(false);

        image.sprite = sprite;

        var translatedCompanyName = Core.Localization.Translate(companyNames[(int)carType]);

        companyName.text = translatedCompanyName;

        price = MathF.Round(price, 2);

        cost.text = $"${price}";

        AssignListener(action);
    }

    public void Hide() => gameObject.SetActive(false);
}
