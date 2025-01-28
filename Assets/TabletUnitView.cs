using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabletUnitView : MonoBehaviour
{
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text pcsPriceText;
    [SerializeField] private TMP_Text qtyText;
    [SerializeField] private TMP_Text allPriceText;
    [SerializeField] private Image statusImage;
    [SerializeField] private Sprite equalStatusSprite;
    [SerializeField] private Sprite unequalStatusSprite;

    public TabletUnitView SetNumber(int number)
    {
        numberText.text = number.ToString();
        return this;
    }

    public TabletUnitView SetName(string name)
    {
        nameText.text = name;
        return this;
    }

    public TabletUnitView SetPcsPriceAndQty(float price, float qty, MeasureType type = MeasureType.pcs)
    {
        pcsPriceText.text = $"{price}$";

        string measure = type == MeasureType.pcs ? "" : Enum.GetName(typeof(MeasureType), type);
        float pcsPrice = MathF.Round(qty, 2);
        float allPrice = MathF.Round(price * qty, 2);

        qtyText.text = $"{pcsPrice}{measure}.";
        allPriceText.text = $"{allPrice}$";
        
        return this;
    }

    public TabletUnitView SetEqualStatus(bool equalStatus)
    {
        statusImage.sprite = equalStatus ? equalStatusSprite : unequalStatusSprite;
        return this;
    }
}

public enum MeasureType
{
    g,
    l,
    ml,
    kg,
    pcs
}
