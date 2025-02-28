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
    [SerializeField] private EndGameWindow endGameWindow;

    [Inject] private ProductFinder productFinder;

    private List<DeliveryConfig> suppliers;
    private float maxLosses;
    private float currentLosses;

    private bool isSupplied;

    public event Action OnLosed;

    private void Awake()
    {
        lossesChecker.OnChecked += UpdateLosses;
        OnLosed += endGameWindow.Show;
    }

    public void SetData(SupplyConfig config)
    {
        suppliers = config.Suppliers.ToList();
        maxLosses = config.MaxLosses;

        UpdateViews();
        deliveryManager.OpenGates(true);
        supplyPresenter.Hide();
        lossesChecker.Hide();
    }

    public void StartSupply(CarType carType)
    {
        if (isSupplied)
            throw new System.Exception("Products have already been supplied");

        supplyPresenter.AssignListener(FinishSupply);

        isSupplied = true;
        supplyPresenter.IsSupplied(isSupplied);

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
    }

    private void AssignListeners(CarSlotView slot)
    {
        UnityAction action = () =>
        {
            UnityAction supplyAction = () => StartSupply(slot.CarType);
            supplyPresenter.SetData(slot.CarType, slot.CarSprite, supplyAction);
        };

        slot.AssignListener(action);
    }

    public void FinishSupply()
    {
        if (!isSupplied)
            throw new Exception("Products hasn't been supplied");

        if(!deliveryManager.HasProducts())
        {
            isSupplied = false;
            SetCarSlotsEnabled(true);
            deliveryManager.FinishDelivering();
            supplyPresenter.IsSupplied(isSupplied);

            if (suppliers.Count == 0)
                deliveryManager.OpenGates(false);

            lossesChecker.Check(orderManager.ActualOrder, orderManager.GetNotes());
        }

        else CluesManager.instance.ShowClue("Before completing the delivery, make sure no products are left in the truck.");
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

    private void UpdateLosses(string name, float amount)
    {
        float different = MathF.Abs(amount);

        if(different >= 0.01f)
        {
            var product = productFinder.FindByName(name);
            float losses = different * product.Price;

            currentLosses += losses;

            if (currentLosses >= maxLosses)
            {
                OnLosed?.Invoke();
            }

            lossesCounter.UpdateData(currentLosses, maxLosses);
        }
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
    }
}
