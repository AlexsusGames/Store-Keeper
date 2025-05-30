using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProductManager : MonoBehaviour, IDataProvider
{
    [SerializeField] private Surface[] staticSurfaces;
    [SerializeField] private Transform storagesParent;
    [SerializeField] private OrderCreator orderManager;

    [Inject] private ProductFinder productFinder;

    public Action<List<string>> OnInitialized;

    private ProductsSpawnInteractor productSpawner;

    private void Awake()
    {
        OnInitialized += orderManager.Init;
    }

    public void Init()
    {
        productSpawner = Core.Interactors.GetInteractor<ProductsSpawnInteractor>();

        var interactor = Core.Interactors.GetInteractor<PricingInteractor>();
        interactor.Setup(productFinder);

        var surfaces = GetActiveSurfaces();

        List<string> products = new();

        for (int i = 0; i < surfaces.Count; i++)
        {
            productSpawner.PlaceProducts(surfaces[i].GetSurfaceId(), surfaces[i].GetSurface(), products);
        }

        OnInitialized?.Invoke(products);
    }

    public void Load()
    {
        Init();
    }

    public void TrySpoilProducts()
    {
        var surfaces = GetActiveSurfaces();

        foreach (var surface in surfaces)
        {
            var parents = surface.Surfaces;
            var storageType = surface.GetStorageType();

            for (int i = 0; i < parents.Count; i++)
            {
                var parent = parents[i];

                for (int j = 0; j < parent.childCount; j++)
                {
                    if (parent.GetChild(j).TryGetComponent(out StoreBox box))
                    {
                        SpoilProduct(storageType, box);
                    }
                }
            }
        }
    }

    private void SpoilProduct(StorageType storageType, StoreBox box)
    {
        var productStorageType = productFinder.FindByName(box.ProductName).StorageType;

        if (storageType != productStorageType && box.IsCanBeSpoiled)
        {
            box.IsSpoilt = true;
            Core.Statistic.OnProductSpoiled();
        }

        if (box.IsHasChild)
        {
            Transform parent = box.ChildPoint;

            for (int j = 0; j < parent.childCount; j++)
            {
                if(parent.GetChild(j).TryGetComponent(out StoreBox childBox))
                {
                    SpoilProduct(storageType, childBox);
                }
            }
        }
    }

    public void Save() => productSpawner.SavePosition(GetActiveSurfaces());

    private List<Surface> GetActiveSurfaces()
    {
        List<Surface> result = new List<Surface>();

        for (int i = 0; i < storagesParent.childCount; i++)
        {
            var child = storagesParent.GetChild(i);

            if (child.TryGetComponent(out Surface surface))
            {
                result.Add(surface);
            }
        }

        result.AddRange(staticSurfaces);

        return result;
    }

    private List<Stack<StoreBox>> GetBoxMap()
    {
        List<Stack<StoreBox>> storeBoxes = new List<Stack<StoreBox>>();

        var surfaces = GetActiveSurfaces();

        foreach (var surface in surfaces)
        {
            var parents = surface.Surfaces;

            for (int i = 0; i < parents.Count; i++)
            {
                var parent = parents[i];

                for (int j = 0; j < parent.childCount; j++)
                {
                    if (parent.GetChild(j).TryGetComponent(out StoreBox box))
                    {
                        Stack<StoreBox> boxColumn = new Stack<StoreBox>();

                        AddProductToStack(boxColumn, box);

                        storeBoxes.Add(boxColumn);
                    }
                }
            }
        }

        return storeBoxes;
    }

    private void AddProductToStack(Stack<StoreBox> stack, StoreBox product)
    {
        stack.Push(product);

        if (product.IsHasChild)
        {
            for (int i = 0; i < product.ChildPoint.childCount; i++)
            {
                var box = product.ChildPoint.GetChild(i);

                if(box.TryGetComponent(out StoreBox storeBox))
                {
                    AddProductToStack(stack, storeBox);
                }
            }
        }
    }

    public DeliveryReport TryLoadDeliveredProducts(Dictionary<string, float> productMap)
    {
        var pricingInteractor = Core.Interactors.GetInteractor<PricingInteractor>();

        var boxColumns = GetBoxMap();

        Dictionary<StoreBox, int> result = new();

        for (int i = 0; i < boxColumns.Count; i++) 
        {
            HandleStack(productMap, boxColumns[i], result);
        }

        foreach(var product in productMap.Keys)
        {
            if (productMap[product] > 0)
            {
                string translatedProduct = Core.Localization.Translate(product);
                string translatedMessage = Core.Localization.Translate("The loader can't find enough:");

                Core.Clues.Show($"{translatedMessage} {translatedProduct}");
                
                return new DeliveryReport(isSuccess: false);
            }
        }

        Dictionary<string, float> spoiltProducts = new();

        bool hasPriceChanged = false;
        bool hasSpoilt = false;

        foreach (var unit in result)
        {
            var box = unit.Key;
            var amountToRemove = unit.Value;

            if (!hasPriceChanged)
                hasPriceChanged = pricingInteractor.WasChanged(box.ProductName);

            float spoiltAmount = 0;

            if (box.GetItemsAmount() >= amountToRemove)
            {
                Core.ProductList.RemoveProduct(box);

                if(box.IsSpoilt)
                    spoiltAmount = box.GetItemsAmount();

                Destroy(box.gameObject);
            }
            else
            {
                float totalAmount = box.UseWeight ? amountToRemove * box.ProductWeight : amountToRemove;

                if (box.IsSpoilt)
                    spoiltAmount = amountToRemove;

                Core.ProductList.RemoveProduct(box.ProductName, totalAmount);

                box.RemoveItems(amountToRemove);
            }

            spoiltProducts[box.ProductName] = spoiltProducts.GetValueOrDefault(box.ProductName) - spoiltAmount;
        }

        return new DeliveryReport(spoiltProducts, hasSpoilt, hasPriceChanged);
    }

    private void HandleStack(Dictionary<string, float> productMap, Stack<StoreBox> boxColumn, Dictionary<StoreBox, int> result)
    {
        var productBox = boxColumn.Peek();

        if (!productMap.ContainsKey(productBox.ProductName))
            return;

        if (productMap[productBox.ProductName] == 0)
            return;

        var productAmount = productBox.GetItemsAmount();

        var totalAmount = productBox.UseWeight ? productBox.TotalWeight : productAmount;
        var orderedAmount = productMap[productBox.ProductName];

        if (totalAmount <= orderedAmount)
        {
            productMap[productBox.ProductName] -= totalAmount;
            result[productBox] = productAmount;

            boxColumn.Pop();

            if(boxColumn.Count > 0)
            {
                HandleStack(productMap, boxColumn, result);
            }
        }
        else
        {
            float amountToRemove = productBox.UseWeight ? orderedAmount / productBox.ProductWeight : orderedAmount;

            productMap[productBox.ProductName] = 0;
            result[productBox] = Mathf.RoundToInt(amountToRemove);
        }
    }
}
