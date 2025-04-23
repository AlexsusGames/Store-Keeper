using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyRating : MonoBehaviour
{
    [SerializeField] private CompanyRatingView[] views;

    private Dictionary<CarType, CompanyRatingView> ratingMap;

    private void Awake()
    {
        ratingMap = new();

        for (int i = 0; i < views.Length; i++)
        {
            ratingMap[(CarType)i] = views[i];
        }
    }

    private void Start()
    {
        foreach (var view in views)
        {
            view.UpdateView();
        }
    }

    public void UpdateView(CarType type)
    {
        Debug.Log(ratingMap[type].name);

        ratingMap[type].UpdateView();
    }
}
