using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class DeliveringManager : MonoBehaviour
{
    [SerializeField] private OrderManager orderManager;

    [SerializeField] private Tablet playersTablet;
    [SerializeField] private GameObject tableTablet;

    [SerializeField] private PalletController palletController;
    [SerializeField] private ProductManager productManager;

    [SerializeField] private EmployeeManager employeeManager;

    [Inject] private ProductFinder productFinder;

    public event Action<DeliveryReport> DeliveryReport;

    public event Action<CarType> OnCarArrived;
    public event Action OnCarDelivered;

    private Dictionary<string, float> expectedProducts;
    private Dictionary<string, float> actualProducts;

    public bool isCarArrived;
    private bool isDelivering;


    private void CarArrive(CarType type, bool isDelivering)
    {
        palletController.ClearPallets();

        OnCarArrived?.Invoke(type);

        expectedProducts = new();
        actualProducts = new();

        isCarArrived = true;

        this.isDelivering = isDelivering;
    }

    public bool TryDeliverProducts(DeliveryData deliveryData)
    {
        if (isDelivering)
        {
            Core.Clues.Show("The truck has already arrived");
            return false;
        }

        if (isCarArrived)
        {
            Core.Clues.Show("You cannot start a delivery during a shipment.");
            return false;
        }

        CarArrive(CarType.Delivery, true);

        palletController.CreateEmptyPallets();

        if(deliveryData != null)
        {
            tableTablet.SetActive(true);

            for (int i = 0; i < deliveryData.OrderedProducts.Count; i++)
            {
                var order = deliveryData.OrderedProducts[i];

                expectedProducts[order.Product] = order.Amount;
            }

            actualProducts = expectedProducts;

            orderManager.Init(expectedProducts, actualProducts, CarType.Delivery);
        }

        return true;
    }

    public bool DeliverToShop(ShopInteractor interactor, StoreConfig data)
    {
        Dictionary<string, float> loadedProducts = palletController.GetLoadedProducts();

        if(loadedProducts.Count == 0)
        {
            Core.Clues.Show("You cannot send out an empty truck.");
            return false;
        }

        foreach (var product in loadedProducts.Keys)
        {
            var config = productFinder.FindByName(product);

            if (!config.CompanyTypes.Contains(data.CompanyType))
            {
                string msgTranslated = Core.Localization.Translate("Some of the loaded items do not match the store type.");
                string productTranslated = Core.Localization.Translate(product);

                Core.Clues.Show($"{msgTranslated} {productTranslated}");
                return false;
            }
        }

        foreach (var product in loadedProducts.Keys)
        {
            var amount = loadedProducts[product];

            interactor.AddProduct(data.Id, product, amount);

            Core.ProductList.RemoveProduct(product, amount);
        }

        isDelivering = false;

        LetTruck();

        return true;
    }

    public bool TryFinishDeliveryByLoader()
    {
        if (HasProducts())
        {
            Core.Clues.Show("You can't assign the loader to load the truck if you've already started doing it yourself");
            return false;
        }

        Dictionary<string, float> order = new(expectedProducts);

        var report = productManager.TryLoadDeliveredProducts(order);

        return TryFinishDelivery(report);
    }

    private bool TryFinishDelivery(DeliveryReport report)
    {
        if (report.IsDeliverySent)
        {
            DeliveryReport?.Invoke(report);

            isDelivering = false;

            LetTruck();
        }

        return report.IsDeliverySent;
    }

    public bool TryFinishDelivery()
    {
        var report = palletController.GetDeliveryReport(expectedProducts);

        return TryFinishDelivery(report);
    }
    public void FinishDeliveryByLoader()
    {
        if (employeeManager.IsHired(EmployeeType.Loader))
        {
            TryFinishDeliveryByLoader();
        }
        else Core.Clues.Show("You need to hire the loader before you can tell him to load the truck.");
    }
    public void FinishDelivery() => TryFinishDelivery();

    public bool TrySupplyProducts(DeliveryConfig deliveryData)
    {
        if(isCarArrived)
        {
            Core.Clues.Show("You cannot start a shipment during a delivery.");
            return false;
        }

        CarArrive(deliveryData.carType, false);

        tableTablet.SetActive(true);

        for (int i = 0; i < deliveryData.pallets.Length; i++)
        {
            var parent = palletController.GetPalletParent(i);
            var pallet = deliveryData.pallets[i].GetOrderDelivered(parent);

            palletController.AddPallet(pallet);

            var order = pallet.GetOrder();

            foreach (var item in order.Keys)
            {
                float current = 0;

                if(expectedProducts.ContainsKey(item))
                    current = expectedProducts[item];

                expectedProducts[item] = MathF.Round(order[item] + current, 2);
            }

            var changedOrder = deliveryData.pallets[i].GetChangedOrderByDifficult(pallet);

            foreach(var item in changedOrder.Keys)
            {
                float current = 0;

                if (actualProducts.ContainsKey(item))
                    current = actualProducts[item];

                Core.ProductList.AddProduct(item, changedOrder[item]);

                actualProducts[item] = MathF.Round(changedOrder[item] + current, 2);
            }
        }

        orderManager.Init(expectedProducts, actualProducts, deliveryData.carType);

        return true;
    }

    public bool SwapObjectsPositions() => palletController.Swapped();
    public bool HasProducts() => palletController.HasProducts();

    public bool CancelDelivering()
    {
        if(!isCarArrived)
        {
            return false;
        }

        if (palletController.HasProducts())
        {
            Core.Clues.Show("To cancel the delivery, you need to unload the products you just loaded.");
            return false;
        }

        isDelivering = false;

        LetTruck();

        return true;
    }

    public void LetTruck()
    {
        if (isCarArrived)
        {
            playersTablet.ChangeClipboardEnabled(false);
            tableTablet.SetActive(false);

            OnCarDelivered?.Invoke();

            isCarArrived = false;
        }
    }
}
