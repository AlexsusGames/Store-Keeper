using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class ProductSupplyManager : MonoBehaviour
{
    [SerializeField] private DeliveringManager deliveryManager;
    [SerializeField] private OrderManager orderManager;
    [SerializeField] private CompanyRating companyRating;
    [SerializeField] private OrderCreator orderCreator;

    [SerializeField] private SupplyPresenter supplyPresenter;
    [SerializeField] private LossesChecker lossesChecker;
    [SerializeField] private CarSlotView[] carSlotView;
    [SerializeField] private PhoneController phone;
    [SerializeField] private GameEntryPoint gameEntryPoint;

    [Inject] private SupplyCreator supplyCreator;

    private DayProgressInteractor dayProgressInteractor;

    private List<DeliveryConfig> suppliers;

    private DeliveryConfig cachedDeliveryConfig;
    public bool IsBeingSupplied { get; private set; }

    private void Awake()
    {
        lossesChecker.OnChecked += OnChecked;
    }

    public void SetData(List<string> suppliersID)
    {
        dayProgressInteractor = Core.Interactors.GetInteractor<DayProgressInteractor>();
        var day = Core.Statistic.GetDaysPassed();

        List<string> data;

        if (dayProgressInteractor.IsHasSavedData(day))
        {
            data = dayProgressInteractor.GetData();
        }
        else
        {
            data = suppliersID;
            dayProgressInteractor.SetData(data, day);
        }

        CreateSuppliersList(data);

        UpdateViews();

        supplyPresenter.Hide();
        lossesChecker.Hide();
    }

    public bool StartSupply(CarType carType)
    {
        if (IsBeingSupplied)
            return false;

        DeliveryConfig config = null;

        for (int i = 0; i < suppliers.Count; i++)
        {
            config = suppliers[i];

            if (config.carType == carType)
            {
                break;
            }
        }

        if (config == null)
            throw new Exception($"CarType: {carType} is missing");

        if (deliveryManager.TrySupplyProducts(config))
        {
            cachedDeliveryConfig = config;

            supplyPresenter.AssignListener(FinishSupply);

            IsBeingSupplied = true;
            supplyPresenter.IsSupplied(IsBeingSupplied);

            suppliers.Remove(config);

            UpdateViews();

            SetCarSlotsEnabled(false);

            orderCreator.UpdateView();

            Core.Quest.TryChangeQuest(QuestType.OrderShipment, 1);
            return true;
        }
        else return false;
    }

    private void CreateSuppliersList(List<string> suppliers)
    {
        this.suppliers = new();

        foreach (string supplier in suppliers)
        {
            var config = supplyCreator.GetConfigByID(supplier);
            this.suppliers.Add(config);
        }
    }

    private void AssignListeners(CarSlotView slot, float price)
    {
        UnityAction action = () =>
        {
            UnityAction supplyAction = () =>
            {
                if (price <= Bank.MoneyAmount)
                {
                    if (StartSupply(slot.CarType))
                    {
                        Bank.Spend(this, price);
                    }
                }
                else Core.Clues.Show("Not enough money to pay for the shipment.");

                Core.Sound.PlayClip(AudioType.MouseClick);
            };

            supplyPresenter.SetData(slot.CarType, slot.CarSprite, supplyAction, price);
            Core.Sound.PlayClip(AudioType.MouseClick);
        };

        slot.AssignListener(action);
    }

    public void FinishSupply()
    {
        Core.Sound.PlayClip(AudioType.MouseClick);

        if (!orderManager.IsFilledIn())
        {
            Core.Clues.Show("Before completing the delivery, make sure that all notes in the invoice are filled in.");
            return;
        }

        if (!IsBeingSupplied)
            throw new Exception("Products hasn't been supplied");

        if(!deliveryManager.HasProducts())
        {
            SetCarSlotsEnabled(true);
            deliveryManager.LetTruck();

            lossesChecker.Check(orderManager.ActualOrder, orderManager.GetNotes(), orderManager.ExpectedOrder);
        }

        else Core.Clues.Show("Before completing the delivery, make sure no products are left in the truck.");
    }

    private void OnChecked(float saved, float totalPrice, float losses)
    {
        Core.Statistic.OnSupply(cachedDeliveryConfig.carType, totalPrice, losses);
        Bank.AddCoins(this, saved);

        UnityAction firstAction = SaveProgress;
        UnityAction secondAction = losses > 0 ? phone.Replay : null;

        BotDialogCreator dialogCreator = new BotDialogCreator();
        var dialogConfig = dialogCreator.GetLossesReport(losses, totalPrice / 10);

        phone.OpenMessenger(dialogConfig, firstAction, secondAction);
    }

    private void SaveProgress()
    {
        var data = Core.Interactors.GetInteractor<DayProgressInteractor>();
        data.CompleteCar(cachedDeliveryConfig.DeliveryID);
        gameEntryPoint.SaveData();

        IsBeingSupplied = false;

        phone.ClosePhone();
        Core.Quest.TryChangeQuest(QuestType.FinishSupply);

        companyRating.UpdateView(cachedDeliveryConfig.carType);
    }

    private void SetCarSlotsEnabled(bool value)
    {
        for (int i = 0; i < carSlotView.Length; i++)
        {
            carSlotView[i].Interactable = value;
        }

        if (value)
            supplyPresenter.Hide();
    }

    private void UpdateViews()
    {
        for (int i = 0; i < carSlotView.Length; i++)
        {
            carSlotView[i].Hide();
        }

        for (int i = 0; i < suppliers.Count; i++)
        {
            int index = i;
            carSlotView[i].SetData(suppliers[index].pallets.Length, suppliers[i].carType);
            AssignListeners(carSlotView[index], suppliers[index].GetCost());
        }
    }
}
