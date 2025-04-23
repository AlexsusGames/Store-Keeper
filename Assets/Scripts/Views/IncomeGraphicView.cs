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

    [SerializeField] private Color incomeColor;
    [SerializeField] private Color lossesColor;

    public void SetData(float income, int highest, int day)
    {
        float sliderValue = income / highest;

        slider.value = sliderValue;

        incomeText.color = income > 0 ? incomeColor : lossesColor;

        string dayTranslated = Core.Localization.Translate("Day");

        dayText.text = day > 0 ? $"{dayTranslated} {day}" : "";
        incomeText.text = income == 0 ? "" : $"${(int)income}";
    }
}
