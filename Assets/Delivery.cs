using System;
using System.Collections;
using System.Collections.Generic;
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
        if (deliveryManager.TryDeliverProducts(deliveryData))
        {
            currentDelivery = deliveryData;
            orderPresenter.ChangeDeliveryButtonEnabled(true);
        }
    }

    private void OnFinishDelivery(Dictionary<string, float> report, bool wereSpoiled)
    {
        var price = CalculatePrice(report);

        var dialog = botDialogCreator.GetDeliveryReport(report, wereSpoiled, orderPresenter.CurrentCompany, callerSprite);
        var interactor = Core.Interactors.GetInteractor<DeliveryInteractor>();

        UnityAction first = () =>
        {
            interactor.FinishDelivery(currentDelivery);

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
        float price = currentDelivery.GetPrice(productFinder);

        foreach(var item in report.Keys)
        {
            if (report[item] < 0)
            {
                var config = productFinder.FindByName(item);

                float sum = config.Price * Math.Abs(report[item]);

                losses += sum;
            }
        }

        price -= (losses * 2);

        return price;
    }
}
