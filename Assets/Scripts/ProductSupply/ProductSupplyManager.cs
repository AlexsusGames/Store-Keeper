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

    [SerializeField] private SupplyPresenter supplyPresenter;
    [SerializeField] private LossesCounter lossesCounter;
    [SerializeField] private LossesChecker lossesChecker;
    [SerializeField] private CarSlotView[] carSlotView;
    [SerializeField] private PhoneController phone;
    [SerializeField] private GameEntryPoint gameEntryPoint;

    private DayProgressInteractor dayProgressInteractor;

    private List<DeliveryConfig> suppliers;
    private CarType cachedCarType;

    private float currentLosses;
    private float maxLosses;

    private bool isBeingSupplied;

    private void Awake()
    {
        lossesChecker.OnChecked += OnChecked;
    }

    public void SetData(SupplyConfig config)
    {
        suppliers = config.Suppliers.ToList();
        maxLosses = config.MaxLosses;

        dayProgressInteractor = Core.Interactors.GetInteractor<DayProgressInteractor>();

        if(dayProgressInteractor.IsHasSavedData())
        {
            currentLosses = dayProgressInteractor.GetCurrentLosses();
            dayProgressInteractor.SubstractCompletedCars(suppliers);
        }

        UpdateViews();

        deliveryManager.OpenGates(true);
        supplyPresenter.Hide();
        lossesChecker.Hide();
    }

    public void StartSupply(CarType carType)
    {
        if (isBeingSupplied)
            return;

        cachedCarType = carType;

        supplyPresenter.AssignListener(FinishSupply);

        isBeingSupplied = true;
        supplyPresenter.IsSupplied(isBeingSupplied);

        DeliveryConfig config = null;

        for (int i = 0; i < suppliers.Count; i++)
        {
            if (suppliers[i].carType == carType)
            {
                config = suppliers[i];
                break;
            }
        }
        
        if(config == null)
            throw new Exception($"CarType: {carType} is missing");

        suppliers.Remove(config);

        deliveryManager.DeliverProducts(config);
        UpdateViews();

        SetCarSlotsEnabled(false);

        Core.Quest.TryChangeQuest(QuestType.OrderShipment, 1);
    }

    private void AssignListeners(CarSlotView slot)
    {
        UnityAction action = () =>
        {
            UnityAction supplyAction = () =>
            {
                StartSupply(slot.CarType);
                Core.Sound.PlayClip(AudioType.MouseClick);
            };

            supplyPresenter.SetData(slot.CarType, slot.CarSprite, supplyAction);
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

        if (!isBeingSupplied)
            throw new Exception("Products hasn't been supplied");

        if(!deliveryManager.HasProducts())
        {
            SetCarSlotsEnabled(true);
            deliveryManager.FinishDelivering();
            supplyPresenter.IsSupplied(isBeingSupplied);

            if (suppliers.Count == 0)
                deliveryManager.OpenGates(false);

            lossesChecker.Check(orderManager.ActualOrder, orderManager.GetNotes());
        }

        else Core.Clues.Show("Before completing the delivery, make sure no products are left in the truck.");
    }

    private void OnChecked(float losses, float total)
    {
        Core.Statistic.OnSupply(cachedCarType, total, losses);
        Bank.Spend(this, total + losses);

        currentLosses += losses;

        lossesCounter.UpdateData(currentLosses, maxLosses);

        UnityAction firstAction = SaveProgress;
        UnityAction secondAction = currentLosses >= maxLosses ? phone.Replay : null;

        BotDialogCreator dialogCreator = new BotDialogCreator();
        var dialogConfig = dialogCreator.GetLossesReport(losses, maxLosses);

        dayProgressInteractor.SetLosses(currentLosses);

        lossesCounter.UpdateData(currentLosses, maxLosses);

        phone.OpenMessenger(dialogConfig, firstAction, secondAction);
    }

    private void SaveProgress()
    {
        var data = Core.Interactors.GetInteractor<DayProgressInteractor>();
        data.CompleteCar(cachedCarType);
        gameEntryPoint.SaveData();

        isBeingSupplied = false;

        phone.ClosePhone();
        Core.Quest.TryChangeQuest(QuestType.FinishSupply);
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
            AssignListeners(carSlotView[index]);
        }

        lossesCounter.UpdateData(currentLosses, maxLosses);
    }
}
