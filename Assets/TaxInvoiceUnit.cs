using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaxInvoiceUnit : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;

    public void SetData(string name, float amount)
    {
        gameObject.SetActive(true);

        string translatedName = Core.Localization.Translate(name);

        nameText.text = translatedName;
        costText.text = $"{MathF.Round(amount, 2)}$";
    }

    public void Hide() => gameObject.SetActive(false);
}
