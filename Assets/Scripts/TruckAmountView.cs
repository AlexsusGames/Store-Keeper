using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TruckAmountView : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject[] trucks;

    private DeliveryInteractor interactor;

    public void Init()
    {
        interactor = Core.Interactors.GetInteractor<DeliveryInteractor>();
        UpdateView();

        interactor.OnTruckAmountChanged += UpdateView;
    }

    private void SetActiveTrucksAmount(int amount)
    {
        for (int i = 0; i < trucks.Length; i++)
        {
            if(amount <= i)
            {
                trucks[i].SetActive(false);
            }
            else trucks[i].SetActive(true);
        }
    }

    private void UpdateView()
    {
        int truckAmount = interactor.GetRemainingTrucks();

        text.text = $"{truckAmount} / {interactor.TotalCars}";

        SetActiveTrucksAmount(truckAmount);
    }
}
