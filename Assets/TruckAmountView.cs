using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TruckAmountView : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private DeliveryInteractor interactor;

    private void Start()
    {
        interactor = Core.Interactors.GetInteractor<DeliveryInteractor>();
        UpdateView();

        interactor.OnTruckAmountChanged += UpdateView;
    }

    private void UpdateView()
    {
        text.text = $"{interactor.GetRemainingTrucks()} / {interactor.TotalCars}";
    }
}
