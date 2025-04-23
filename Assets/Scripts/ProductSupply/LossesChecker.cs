using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class LossesChecker : MonoBehaviour
{
    [SerializeField] private TMP_Text[] log;

    [Inject] private ProductFinder productFinder;

    public event Action<float, float, float> OnChecked;

    private const string RED_COLOR = "<color=red>";
    private const string GREEN_COLOR = "<color=green>";

    private List<string> messages;

    public void Check(Dictionary<string, float> actual, Dictionary<string, float> noted, Dictionary<string, float> expected)
    {
        gameObject.SetActive(true);
        messages = new();
        UpdateView();

        StartCoroutine(Timer(actual, noted, expected));
    }
    public void Hide() => gameObject.SetActive(false);

    private IEnumerator Timer(Dictionary<string, float> actual, Dictionary<string, float> noted, Dictionary<string, float> expected)
    {
        float totalLosses = 0;
        float totalPrice = 0;
        float savedMoney = 0;

        foreach (var product in noted.Keys)
        {
            float actualQty = actual[product];
            float notedQty = noted[product];
            float expectedQty = expected[product];

            float difference = actualQty - notedQty;

            var productData = productFinder.FindByName(product);
            float pricePerUnit = productData.Price;

            float itemTotalPrice = actualQty * pricePerUnit;

            float saved = 0;
            float losses = 0;

            var newItem = StringBuilder(product, notedQty, actualQty);

            if (Math.Abs(difference) < 0.1f)
            {
                saved = (expectedQty - actualQty) * pricePerUnit;
            }
            else
            {
                if (notedQty > actualQty)
                {
                    losses = (notedQty - actualQty) * pricePerUnit;
                    saved = (expectedQty - notedQty) * pricePerUnit;
                }
                else
                {
                    losses = (expectedQty - actualQty) * pricePerUnit;
                }
            }

            totalLosses += losses;
            savedMoney += saved;

            totalPrice += itemTotalPrice;

            messages.Insert(0, newItem);
            UpdateView();

            yield return new WaitForSeconds(0.5f);
        }

        OnChecked?.Invoke(MathF.Round(savedMoney, 2), MathF.Round(totalPrice, 2), MathF.Round(totalLosses, 2));

        yield return new WaitForSeconds(2f);
        Hide();
    }

    private string StringBuilder(string productName, float notedQty, float actualQty)
    {
        float difference = actualQty - notedQty;

        string translatedProduct = Core.Localization.Translate(productName);
        string translatedSuccess = Core.Localization.Translate("Successful");
        string translatedFailure = Core.Localization.Translate("Failure");

        string result = Math.Abs(difference) < 0.1f ? $"{GREEN_COLOR}{translatedSuccess}" : $"{RED_COLOR}{translatedFailure}";
        string newItem = $"{translatedProduct} [{notedQty} / {actualQty}] - {result}";

        return newItem;
    }

    private void UpdateView()
    {
        for (int i = 0; i < log.Length; i++)
        {
            if (i >= messages.Count)
                log[i].text = "";

            else log[i].text = messages[i];
        }
    }
}
