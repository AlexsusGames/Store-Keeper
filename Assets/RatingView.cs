using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RatingView : MonoBehaviour
{
    [SerializeField] private TMP_Text ratingText;
    [SerializeField] private Image ratingBar;

    private DayProgressInteractor interactor;

    private void Awake()
    {
        interactor = Core.Interactors.GetInteractor<DayProgressInteractor>();

        interactor.OnRatingChanged += UpdateRating;

        UpdateRating();
    }

    private void UpdateRating()
    {
        var rating = interactor.GetRating();

        float progress = rating % 1000;
        int level = rating / 1000;

        ratingBar.fillAmount = progress / 1000;
        ratingText.text = level.ToString();
    }
}
