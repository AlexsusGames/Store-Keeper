using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Delivery : MonoBehaviour
{
    [SerializeField] private OrderPresenter orderPresenter;
    [SerializeField] private DeliveringManager deliveryManager;
    [SerializeField] private OrderCreator orderCreator;

    [SerializeField] private PhoneController phoneController;
    [SerializeField] private GameEntryPoint gameEntryPoint;

    [SerializeField] private Sprite callerSprite;

    [Inject] private ProductFinder productFinder;

    private BotDialogCreator botDialogCreator;
    private DeliveryData currentDelivery;

    private void Awake()
    {
        orderPresenter.OnStartDelivery += StartDelivery;

        deliveryManager.DeliveryReport += OnFinishDelivery;

        botDialogCreator = new BotDialogCreator();
    }

    private void StartDelivery(DeliveryData deliveryData)
    {
        var interactor = Core.Interactors.GetInteractor<DeliveryInteractor>();

        if (deliveryManager.TryDeliverProducts(deliveryData))
        {
            interactor.OnStartDelivery(deliveryData);

            currentDelivery = deliveryData;
            orderPresenter.ChangeDeliveryButtonEnabled(true);
        }
    }

    public void CancelDelivery()
    {
        if(deliveryManager.CancelDelivering())
        {
            var interactor = Core.Interactors.GetInteractor<DeliveryInteractor>();

            interactor.CancelDelivery(currentDelivery);

            orderCreator.UpdateView();

            orderPresenter.Hide();
        }
    }

    private void OnFinishDelivery(DeliveryReport report)
    {
        var price = CalculatePrice(report.Products);

        var dialog = botDialogCreator.GetDeliveryReport(report, orderPresenter.CurrentCompany, callerSprite);

        UnityAction first = () =>
        {
            orderCreator.UpdateView();

            Bank.AddCoins(this, price);

            Core.Statistic.OnBoxSold(currentDelivery.BoxCount, price);
            Core.Quest.TryChangeQuest(QuestType.FinishDelivery);

            gameEntryPoint.SaveData();

            orderPresenter.Hide();
            phoneController.ClosePhone();
        };

        UnityAction second = () =>
        {
            phoneController.Replay();
        };

        phoneController.OpenMessenger(dialog, first, second);
    }

    private float CalculatePrice(Dictionary<string, float> report)
    {
        float losses = 0;
        float price = currentDelivery.GetPrice();

        var interactor = Core.Interactors.GetInteractor<PricingInteractor>();

        foreach(var item in report.Keys)
        {
            if (report[item] < 0)
            {
                var config = productFinder.FindByName(item);

                if (report[item] < 0)
                {
                    float sum = interactor.GetDeliveryPrice(item) * Math.Abs(report[item]);

                    losses += sum;
                }
            }
        }

        price -= losses;

        return price;
    }
}
