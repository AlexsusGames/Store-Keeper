using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IncomeGraphic : MonoBehaviour
{
    [SerializeField] private IncomeGraphicView[] views;
    [SerializeField] private TMP_Text highestValueText;

    [SerializeField] private string id;

    private void OnEnable()
    {
        if (!string.IsNullOrEmpty(id))
        {
            UpdateInfo(id);
        }
        else gameObject.SetActive(false);
    }

    public void SetData(string id)
    {
        this.id = id;

        gameObject.SetActive(true);
    }

    private void UpdateInfo(string id)
    {
        gameObject.SetActive(true);

        var week = Core.Statistic.GetIncomeWeek(id);

        int highest = 1000;

        for (int i = 0; i < week.Count; i++)
        {
            if (week[i] > highest)
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

    public void Hide() => gameObject.SetActive(false);
}
