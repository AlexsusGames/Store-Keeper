using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductsManager 
{
    private StorageDataProvider storageDataProvider = new();
    private Dictionary<string, List<StorageData>> storageProducts;

    private ProductFinder productFinder;

    public ProductsManager(ProductFinder productFinder)
    {
        this.productFinder = productFinder;
    }

    private void Init()
    {
        var data = storageDataProvider.GetData();
        storageProducts = new();

        for (int i = 0; i < data.Count; i++)
        {
            string id = data[i].StorageId;

            if (!storageProducts.ContainsKey(id))
            {
                storageProducts[id] = new();
            }

            storageProducts[id].Add(data[i]);
        }
    }

    public void PlaceProducts(string id, Transform parent)
    {
        if (storageProducts == null)
            Init();

        if (!storageProducts.ContainsKey(id))
            return;

        var products = storageProducts[id];

        foreach (var product in products)
        {
            if (product.ProductName == string.Empty)
                continue;

            SpawnProduct(parent, product);
        }
    }

    private void SpawnProduct(Transform parent, StorageData data)
    {
        Debug.Log($"Product Data Child is null - {data.Child == null}");

        var config = productFinder.FindByName(data.ProductName);

        var prefab = config.GetPrefab(parent);

        prefab.transform.position = data.Position;
        prefab.transform.localRotation = data.Rotation;

        if(prefab.TryGetComponent(out Box box))
        {
            box.Init(data.ProductCount);
        }

        if (data.Child != null)
        {
            var childData = data.Child;
            var storeBox = prefab.GetComponent<StoreBox>();

            SpawnProduct(storeBox.ChildPoint, childData);
        }
    }

    public void SavePosition(List<Surface> surfaceList)
    {
        List<StorageData> data = new();

        for (int i = 0; i < surfaceList.Count; i++)
        {
            var surfaces = surfaceList[i].Surfaces;

            for (int j = 0; j < surfaces.Count; j++)
            {
                int childCount = surfaces[j].childCount;

                for (int k = 0; k < childCount; k++)
                {
                    var productData = CreateProductData(surfaces[j].GetChild(k).gameObject, surfaceList[i]);

                    Debug.Log($"Product Data Child is null - {productData.Child == null}");

                    data.Add(productData);
                }
            }
        }

        storageDataProvider.SaveData(new StorageDataList() { list = data });
    }

    private StorageData CreateProductData(GameObject obj, Surface surface)
    {
        if (obj.TryGetComponent(out StoreBox box))
        {
            StorageData storageData = new StorageData();

            storageData.ProductName = box.ProductName;
            storageData.ProductCount = box.GetItemsAmount();

            if(surface != null)
            {
                storageData.StorageId = surface.GetSurfaceId();
            }

            storageData.Position = box.transform.position;
            storageData.Rotation = box.transform.localRotation;

            if(box.IsHasChild)
            {
                storageData.Child = CreateProductData(box.ChilBox.gameObject, null);
            }

            return storageData;
        }

        throw new System.Exception($"Object: {obj.name} doesn't contain 'Store Box' script");
    }
}
