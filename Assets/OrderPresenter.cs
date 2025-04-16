using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class OrderPresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text companyName;
    [SerializeField] private TMP_Text companyMessage;
    [SerializeField] private TMP_Text[] orders;

    [SerializeField] private Button startDeliveryButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button completeButton;
    public string CurrentCompany => companyName.text;
    private string[] measures = { "", "g", "l", "ml", "kg" };

    [Inject] private ProductFinder productFinder;

    public event Action<DeliveryData> OnStartDelivery;

    private CompanyInfo companyInfo = new();

    private void Awake()
    {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
    public void Hide() => gameObject.SetActive(false);

    public void SetData(DeliveryData data)
    {
        gameObject.SetActive(true);

        ResetInfo();

        companyName.text = companyInfo.GetCompanyName(data.CompanyType);
        companyMessage.text = $"{companyInfo.GetRandomCompanyMessage()} \nWe pay: $<color=green>{data.GetPrice(productFinder)}";

        for (int i = 0; i < data.OrderedProducts.Count; i++)
        {
            var orderedProduct = data.OrderedProducts[i];
            var measureType = (int)productFinder.FindByName(orderedProduct.Product).MeasureType;

            string result = $"- {orderedProduct.Product} - {orderedProduct.Amount}{measures[measureType]}";
            orders[i].text = result;
        }

        AssignButton(data);
    }

    public void ChangeDeliveryButtonEnabled(bool enabled)
    {
        startDeliveryButton.interactable = !enabled;
        closeButton.interactable = !enabled;

        completeButton.gameObject.SetActive(enabled);
    }

    private void ResetInfo()
    {
        ChangeDeliveryButtonEnabled(false);

        for (int i = 0;i < orders.Length;i++)
        {
            orders[i].text = "";
        }
    }

    private void AssignButton(DeliveryData data)
    {
        startDeliveryButton.onClick.RemoveAllListeners();

        UnityAction action = () =>
        {
            var interactor = Core.Interactors.GetInteractor<DeliveryInteractor>();

            if (interactor.GetRemainingTrucks() > 0)
            {
                OnStartDelivery?.Invoke(data);
            }
            else Core.Clues.Show("All trucks are currently in use. A truck will be available the next day after delivery.");

            Core.Sound.PlayClip(AudioType.MouseClick);
        };

        startDeliveryButton.onClick.AddListener(action);
    }
}
