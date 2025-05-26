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

    private Coroutine animationRoutine;

    public void SetData(float income, int highest, int day)
    {
        float targetSliderValue = highest > 0 ? income / highest : 0;
        string dayTranslated = Core.Localization.Translate("Day");

        dayText.text = day > 0 ? $"{dayTranslated} {day}" : "";

        if (animationRoutine != null)
            StopCoroutine(animationRoutine);

        animationRoutine = StartCoroutine(AnimateIncomeDisplay(income, targetSliderValue));
    }

    private IEnumerator AnimateIncomeDisplay(float targetIncome, float targetSliderValue)
    {
        float duration = 0.5f;
        float time = 0f;

        float startSliderValue = slider.value;
        float startIncome = GetCurrentIncomeFromText();

        incomeText.color = targetIncome > 0 ? incomeColor : lossesColor;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            slider.value = Mathf.Lerp(startSliderValue, targetSliderValue, t);
            float currentIncome = Mathf.Lerp(startIncome, targetIncome, t);

            incomeText.text = targetIncome == 0 ? "" : $"${(int)currentIncome}";

            yield return null;
        }

        slider.value = targetSliderValue;
        incomeText.text = targetIncome == 0 ? "" : $"${(int)targetIncome}";
        animationRoutine = null;
    }

    private float GetCurrentIncomeFromText()
    {
        if (string.IsNullOrEmpty(incomeText.text))
            return 0;

        string numeric = incomeText.text.Replace("$", "");
        return int.TryParse(numeric, out int value) ? value : 0;
    }
}

