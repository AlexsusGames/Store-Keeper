using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopActionButton actionButton;
    [SerializeField] private ShopRentInfo shopRentInfo;
    [SerializeField] private EmployeeListView employeeListView;
    [SerializeField] private IncomeGraphic incomeGraphic;
    [SerializeField] private RatingBlock ratingBlock;
    [SerializeField] private EmployeeInfo employeeInfo;
    [SerializeField] private ShopSupplyManager shopSupplyManager;

    private bool supplyStarted;

    private StoreConfig config;
    private ShopInteractor interactor;

    private void Awake()
    {
        employeeInfo.OnEmploymentChanged += UpdateView;
    }

    public void SetData(StoreConfig config)
    {
        if(!supplyStarted)
        {
            this.config = config;

            if (interactor == null)
                interactor = Core.Interactors.GetInteractor<ShopInteractor>();

            UpdateView();
        }
    }

    public void PaySalary()
    {
        Core.Sound.PlayClip(AudioType.MouseClick);

        if(!interactor.TryPaySalary(config.Id))
            Core.Clues.Show("Payment failed, not enough money!");

        else shopRentInfo.SetData(config, interactor);
    }

    public void PayRent()
    {
        Core.Sound.PlayClip(AudioType.MouseClick);

        if (!interactor.TryPayRent(config.Id))
            Core.Clues.Show("Payment failed, not enough money!");

        else shopRentInfo.SetData(config, interactor);
    }

    public void UpdateView()
    {
        Core.Sound.PlayClip(AudioType.MouseClick);

        Hide();

        employeeInfo.Init(config, interactor);
        employeeListView.UpdateView(config, interactor);
        shopRentInfo.SetData(config, interactor);
        incomeGraphic.SetData(config.Id);
        shopSupplyManager.Init(interactor, config);

        int rating = Core.Interactors.GetInteractor<DayProgressInteractor>().GetRating();
        bool blockEnabled = rating < config.RequiredRating;

        ratingBlock.Block(config.RequiredRating, blockEnabled);

        if (IsRented()) actionButton.SetDeliveryAction(OrderSupply);
        else actionButton.SetRentAction(() => ChangeRent(true));

        if (config.RentCost == 0) actionButton.Hide();
    }

    public void ChangeRent(bool value)
    {
        if(interactor.HasHired(config.Id))
        {
            Core.Clues.Show("To close the place down, you'll have to lay off all the employees.");
            return;
        }

        interactor.ChangeRent(config.Id, value);

        UpdateView();
    }

    public void CancelSupply()
    {
        supplyStarted = false;
    }

    private void OrderSupply()
    {
        if (shopSupplyManager.TryStartSupply())
        {
            supplyStarted = true;
        }
    }

    private void Hide()
    {
        employeeInfo.Cancel();
        employeeListView.Hide();
        shopRentInfo.Hide();
        incomeGraphic.Hide();
    }
    private bool IsRented() => interactor.IsRenting(config.Id);
}
