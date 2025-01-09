using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductConfig", menuName = "Create/ProductConfig")]
public class ProductConfig : ScriptableObject
{
    [SerializeField] private GameObject prefab;

    public string ProductName;
    public StorageType StorageType;

    public GameObject GetPrefab(Transform parent)
    {
        GameObject obj = Instantiate(prefab.gameObject, parent);
        obj.TryGetComponent(out Box box);
        box.ProductName = ProductName;

        return obj;
    }
}

public enum StorageType
{
    None,
    Fridge,
    Freezer
}
