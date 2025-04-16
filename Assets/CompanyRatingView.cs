using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompanyRatingView : MonoBehaviour
{
    [SerializeField] private CarType carType;
    [SerializeField] private TMP_Text ratingText;
    [SerializeField] private Image ratingBar;

    public CarType CarType => carType;

    public void UpdateView()
    {
        var statistic = Core.Statistic;

        int calculatedRating = (int)(statistic.GetSupplierRating(carType) * 100);

        ratingBar.fillAmount = statistic.GetSupplierRating(carType);

        ratingText.text = $"{calculatedRating} / 100%";
    }
}
