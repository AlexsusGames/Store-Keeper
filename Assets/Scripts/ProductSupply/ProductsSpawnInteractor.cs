using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductsSpawnInteractor : Interactor
{
    private Dictionary<string, List<StorageData>> storageProductsMap;

    private ProductsPositionDataProvider storageDataProvider;
    private ProductFinder productFinder;

    public override void OnCreate()
    {
        productFinder = Resources.Load<ProductFinder>("ProductFinder/ProductFinder");
        storageDataProvider = Core.DataProviders.GetDataProvider<ProductsPositionDataProvider>();
    }

    public override void Init()
    {
        var data = storageDataProvider.Data.list;

        storageProductsMap = new();

        for (int i = 0; i < data.Count; i++)
        {
            string id = data[i].StorageId;

            if (!storageProductsMap.ContainsKey(id))
            {
                storageProductsMap[id] = new();
            }

            storageProductsMap[id].Add(data[i]);
        }
    }

    public void PlaceProducts(string id, Transform parent)
    {
        if (!storageProductsMap.ContainsKey(id))
            return;

        var products = storageProductsMap[id];

        SpawnProduct(parent, products);
    }

    private void SpawnProduct(Transform parent, List<StorageData> data)
    {
        for (int i = 0;i < data.Count;i++)
        {
            if (data[i].ProductName == string.Empty)
            {
                continue;
            }

            Debug.Log($"Product Data Child is null - {data[i].Childs == null}");

            var config = productFinder.FindByName(data[i].ProductName);

            var prefab = config.GetPrefab(parent);

            prefab.transform.position = data[i].Position;
            prefab.transform.localRotation = data[i].Rotation;

            if (prefab.TryGetComponent(out Box box))
            {
                box.Init(data[i].ProductCount);
                box.ProductWeight = data[i].ProductWeight;

                box.IsSpoilt = data[i].IsSpoilt;
            }

            if (prefab.TryGetComponent(out PickupObject pickupObject))
            {
                pickupObject.ChangeLayer(3);
            }

            if (data[i].Childs != null)
            {
                var childData = data[i].Childs;
                var storeBox = prefab.GetComponent<StoreBox>();

                SpawnProduct(storeBox.ChildPoint, childData);
            }
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

                    Debug.Log($"Product Data Child is null - {productData.Childs == null}");

                    data.Add(productData);
                }
            }
        }

        storageDataProvider.Data.list = data;
    }

    private StorageData CreateProductData(GameObject obj, Surface surface)
    {
        if (obj.TryGetComponent(out StoreBox box))
        {
            StorageData storageData = new StorageData();

            storageData.ProductName = box.ProductName;
            storageData.ProductCount = box.GetItemsAmount();
            storageData.ProductWeight = box.ProductWeight;
            storageData.IsSpoilt = box.IsSpoilt;

            if(surface != null)
            {
                storageData.StorageId = surface.GetSurfaceId();
            }

            storageData.Position = box.transform.position;
            storageData.Rotation = box.transform.localRotation;

            if(box.IsHasChild)
            {
                List<StorageData> list = new List<StorageData>();

                for (int i = 0; i < box.ChildPoint.childCount; i++)
                {
                    var child = box.ChildPoint.GetChild(i);
                    var data = CreateProductData(child.gameObject, null);
                    list.Add(data);
                }

                storageData.Childs = list;
            }

            return storageData;
        }

        throw new System.Exception($"Object: {obj.name} doesn't contain 'Store Box' script");
    }
}
