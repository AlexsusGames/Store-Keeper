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

    public ProductsManager()
    {
        productFinder = Resources.Load<ProductFinder>("");
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
        if(storageProducts == null)
            Init();

        if (!storageProducts.ContainsKey(id))
            return;

        var products = storageProducts[id];

        foreach (var product in products)
        {
            var config = productFinder.FindByName(product.ProductName);
            var prefab = config.GetPrefab(parent);

            prefab.transform.position = product.Position;
            prefab.transform.localRotation = product.Rotation;

            if(product.Child != null)
            {

            }
        }
    }

    private void SpawnProduct(Transform parent, StorageData data)
    {
        var config = productFinder.FindByName(data.ProductName);
        var prefab = config.GetPrefab(parent);

        prefab.transform.position = data.Position;
        prefab.transform.localRotation = data.Rotation;

        if (data.Child != null)
        {
            var childData = data.Child;
            var box = prefab.GetComponent<StoreBox>();
            
            SpawnProduct(box.ChildPoint, childData);
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
                    data.Add(productData);
                }
            }
        }

        Debug.Log(data.Count);

        storageDataProvider.SaveData(new StorageDataList() { list = data });
    }

    private StorageData CreateProductData(GameObject obj, Surface surface)
    {
        if (obj.TryGetComponent(out StoreBox box))
        {
            StorageData storageData = new StorageData();

            storageData.ProductName = box.ProductName;
            storageData.ProductCount = box.GetItemsAmount();
            storageData.StorageId = surface.GetSurfaceId();
            storageData.Position = box.transform.position;
            storageData.Rotation = box.transform.rotation;

            if(box.IsHasChild)
            {
                box.TryGetComponent(out Surface boxSurface);
                storageData.Child = CreateProductData(box.ChilBox.gameObject, boxSurface);
            }

            return storageData;
        }

        throw new System.Exception($"Object: {obj.name} doesn't contain 'Store Box' script");
    }
}
