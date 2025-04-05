using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IncomeGraphic : MonoBehaviour
{
    [SerializeField] private IncomeGraphicView[] views;
    [SerializeField] private TMP_Text highestValueText;

    private void Start()
    {
        var week = Core.Statistic.GetIncomeWeek();

        int highest = 1000;

        Print(week);

        for (int i = 0; i < week.Count; i++)
        {
            if(week[i] > highest)
                highest = (int)week[i];
        }

        highestValueText.text = $"${highest}";

        int currentDay = Core.Statistic.GetDaysPassed() - 7;

        for (int i = 0; i < views.Length; i++)
        {
            currentDay++;
            views[i].SetData(week[i], highest, currentDay);
        }
    }

    private void Print(List<float> data)
    {
        string result = "";

        for (int i = 0;i < data.Count; i++)
        {
            result += data[i].ToString();
            result += "\n";
        }

        Debug.Log(result);
    }
}
