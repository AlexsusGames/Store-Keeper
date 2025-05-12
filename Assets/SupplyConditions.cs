using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyConditions : MonoBehaviour
{
    [SerializeField] private StoreUnitShopView[] storeUnits;
    [SerializeField] private StoreFurnitureConfig[] furnitureConfig;

    [SerializeField] private GameObject[] conditions;

    private void Start()
    {
        for (int i = 0; i < storeUnits.Length; i++)
        {
            storeUnits[i].SetLocked();
        }
    }

    public CarType[] GetCarsByCompletedConditions()
    {
        CheckData();

        List<CarType> availableCars = new List<CarType>() { CarType.Milk, CarType.Bread };

        if (conditions[0].activeInHierarchy)
        {
            availableCars.Add(CarType.Grocceries);
        }

        if (conditions[1].activeInHierarchy)
        {
            availableCars.Add(CarType.Chemical);
        }

        if (conditions[2].activeInHierarchy && conditions[3].activeInHierarchy)
        {
            availableCars.Add(CarType.Applience);
        }

        return availableCars.ToArray();
    }

    public void CheckData()
    {
        var dataProvider = Core.DataProviders.GetDataProvider<NonPlacedStoragesDataProvider>();

        for (int i = 0; i < storeUnits.Length; i++)
        {
            string id = furnitureConfig[i].Name;

            if (dataProvider.Has(id))
            {
                conditions[i].SetActive(true);

                storeUnits[i].SetChecked();
            }
        }
    }

    public void UnlockSystems(int[] indexes)
    {
        for (int i = 0; i < indexes.Length; i++)
        {
            storeUnits[indexes[i]].SetUnlocked();
        }
    }
}
