using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class OrderCreator : MonoBehaviour
{
    private const int ORDERS_COUNT = 10;

    private const int MIN_ORDER_SIZE = 1;
    private const int MAX_ORDER_SIZE = 6;

    [Inject] private ProductFinder productFinder;
    [SerializeField] private OrderPresenter orderPresenter;
    [SerializeField] private OrderButton[] orderViews;

    private DeliveryInteractor deliveryInteractor;
    private PricingInteractor pricingInteractor;
    private Dictionary<CompanyType, List<string>> productMap;

    private List<string> cachedNames;

    private void CreateMap(List<string> availableProducts)
    {
        var interactor = Core.Interactors.GetInteractor<PricingInteractor>();

        productMap = new();

        for (int i = 0; i < availableProducts.Count; i++)
        {
            var config = productFinder.FindByName(availableProducts[i]);

            if(interactor.IsForSale(config.ProductName))
            {
                for (int j = 0; j < config.CompanyTypes.Length; j++)
                {
                    CompanyType companyType = config.CompanyTypes[j];

                    if (productMap.ContainsKey(companyType))
                    {
                        if (!productMap[companyType].Contains(config.ProductName))
                        {
                            productMap[companyType].Add(config.ProductName);
                        }
                    }
                    else productMap[companyType] = new List<string>() { config.ProductName };
                }
            }
        }
    }

    public void Init(List<string> availableProducts)
    {
        deliveryInteractor = Core.Interactors.GetInteractor<DeliveryInteractor>();
        pricingInteractor = Core.Interactors.GetInteractor<PricingInteractor>(); 

        var day = Core.Interactors.GetInteractor<StatisticInteractor>().GetDaysPassed();

        List<DeliveryData> deliveryData;

        if (deliveryInteractor.HasSave(day))
        {
            deliveryData = deliveryInteractor.GetDeliveryData();
        }
        else
        {
            deliveryData = CreateOrders(availableProducts);
            deliveryInteractor.CreateDeliveryData(deliveryData, day);
        }

        InitButtons(deliveryData);
    }

    public void UpdateView()
    {
        List<DeliveryData> deliveryData;

        deliveryData = deliveryInteractor.GetDeliveryData();

        InitButtons(deliveryData);
    }

    private void InitButtons(List<DeliveryData> data)
    {
        for (int i = 0; i < orderViews.Length; i++)
        {
            if(data == null || i >= data.Count)
            {
                orderViews[i].Enabled = false;
                continue;
            }

            int index = i;

            UnityAction action = () =>
            {
                orderPresenter.SetData(data[index]);
                Core.Sound.PlayClip(AudioType.MouseClick);
            };

            orderViews[index].Enabled = true;
            orderViews[index].CheckProductAvailability(data[index]);
            orderViews[index].AssignAction(action);
        }
    }

    private List<DeliveryData> CreateOrders(List<string> products)
    {
        CreateMap(products);

        List<DeliveryData> orders = new();

        for(int i = 0; i < ORDERS_COUNT; i ++)
        {
            if (productMap.Count > 0)
            {
                var order = i == 0 ? CreateSimpleOrder() : CreateOrder();
                orders.Add(order);
            }
        }

        return orders;
    }

    private DeliveryData CreateSimpleOrder()
    {
        CompanyType randomType = GetRandomKey();

        cachedNames = new List<string>(productMap[randomType]);

        int index = 0;
        var config = productFinder.FindByName(cachedNames[index]);

        var amount = config.MeasureType == MeasureType.pcs ? config.Capacity : config.Capacity * config.RandomWeight;

        DeliveryData order = new DeliveryData()
        {
            BoxCount = 1,
            CompanyType = randomType,
            OrderedProducts = new List<OrderData>()
            {
                new OrderData() {Product = cachedNames[index], Amount = amount},
            }
        };

        return order;
    }

    private DeliveryData CreateOrder()
    {
        CompanyType randomType = GetRandomKey();

        cachedNames = new List<string>(productMap[randomType]);

        int randomSize = UnityEngine.Random.Range(MIN_ORDER_SIZE, MAX_ORDER_SIZE);

        randomSize = Mathf.Min(randomSize, cachedNames.Count);
        int unnecessaryProductsCount = cachedNames.Count - randomSize;

        for (int i = 0; i < unnecessaryProductsCount; i++)
        {
            int random = UnityEngine.Random.Range(0, cachedNames.Count);

            cachedNames.RemoveAt(random);
        }

        List<OrderData> orders = new List<OrderData>();

        int boxCount = 0;

        for (int i = 0; i < cachedNames.Count; i++)
        {
            int maxValue = pricingInteractor.GetDeliveryMultiplier(cachedNames[i]) + 1;

            float randomBoxCount = UnityEngine.Random.Range(1, maxValue);

            var product = productFinder.FindByName(cachedNames[i]);

            boxCount += (int)randomBoxCount;

            var amount = product.MeasureType == MeasureType.pcs ? randomBoxCount * product.Capacity : product.Capacity * product.RandomWeight * randomBoxCount;

            OrderData data = new()
            {
                Product = cachedNames[i],
                Amount = amount,
            };

            orders.Add(data);
        }

        DeliveryData deliveryData = new DeliveryData()
        {
            CompanyType = randomType,
            OrderedProducts = orders,

            BoxCount = boxCount
        };

        return deliveryData;
    }

    private CompanyType GetRandomKey()
    {
        var list = productMap.Keys.ToArray();
        int index = UnityEngine.Random.Range(0, list.Length);
        return list[index];
    }
}
