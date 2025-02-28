using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductConfig", menuName = "Create/ProductConfig")]
public class ProductConfig : ScriptableObject
{
    [SerializeField] private GameObject prefab;

    public int Capacity;
    public string ProductName;
    public float Price;
    public StorageType StorageType;
    public MeasureType MeasureType;

    [SerializeField] private float minWeight;
    [SerializeField] private float maxWeight;
    public float RandomWeight => MathF.Round(UnityEngine.Random.Range(minWeight, maxWeight), 2);

    public GameObject GetPrefab(Transform parent)
    {
        GameObject obj = Instantiate(prefab.gameObject, parent);
        obj.TryGetComponent(out Box box);

        if (box == null)
            throw new System.Exception("Prefab doesn't contain 'Box' script");

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
