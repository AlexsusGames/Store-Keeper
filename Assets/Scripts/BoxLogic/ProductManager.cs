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

        Debug.Log(storageType + " " + productStorageType);

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
}
