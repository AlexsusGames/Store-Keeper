using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreEditor : MonoBehaviour
{
    [SerializeField] private FurnitureInventoryView inventoryView;

    private FurnitureDataProvider dataProvider;

    private void Awake() => dataProvider = new();

    private void Start()
    {
        UpdateView();

        dataProvider.OnDataChange += UpdateView;
    }

    public void StoreFurniture(string id)
    {
        dataProvider.AddFurniture(id);
    }

    private void OnDestroy() => dataProvider.OnDataChange -= UpdateView;

    private void UpdateView()
    {
        var data = dataProvider.GetFurnitureList();
        inventoryView.SetData(data);
    }
}
