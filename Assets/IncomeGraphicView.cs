using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncomeGraphicView : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text incomeText;
    [SerializeField] private TMP_Text dayText;

    public void SetData(float income, int highest, int day)
    {
        float sliderValue = income / highest;

        slider.value = sliderValue;

        dayText.text = day > 0 ? $"Day {day}" : "";
        incomeText.text = $"${income}";
    }
}
