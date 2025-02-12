using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Data;
using System;
using System.Globalization;

public class Calculator : MonoBehaviour
{
    [SerializeField] private TMP_Text line;

    public event Action<float> OnNoted;
    private string calculatorText;

    private void Awake()
    {
        calculatorText = string.Empty;
        UpdateView();
    }
    private void UpdateView() => line.text = calculatorText;

    public void ResetValue()
    {
        calculatorText = string.Empty;
        UpdateView();
    }

    public void AddSymbol(string symbol)
    {
        calculatorText += symbol;
        UpdateView();
    }

    public void RemoveLastSymbol()
    {
        if (calculatorText.Length > 0)
        {
            calculatorText = calculatorText.Remove(calculatorText.Length - 1);
            UpdateView();
        }
    }

    public void Calculate()
    {
        float result;

        try
        {
            result = Convert.ToSingle(new DataTable().Compute(calculatorText, null));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }

        string newValue = Math.Round(result, 2).ToString(CultureInfo.InvariantCulture);

        calculatorText = newValue;
        UpdateView();
    }

    public void Note()
    {
        float result;

        if(float.TryParse(calculatorText, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
        {
            OnNoted?.Invoke(result);
            ResetValue();
        }
    }
}
