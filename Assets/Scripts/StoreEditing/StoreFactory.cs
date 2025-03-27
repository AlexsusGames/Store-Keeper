using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class StoreFactory : MonoBehaviour 
{
    [Inject] private StoreFurnitureConfigFinder configFinder;

    public FurniturePlacementView Create(string id)
    {
        var prefab = configFinder.FindByName(id).prefab;

        var obj = Instantiate(prefab, transform);

        var furniture = obj.GetComponent<FurniturePlacementView>();
        return furniture;
    }

    public FurniturePlacementView Create(string id, Vector3 position)
    {
        var furniture = Create(id);

        furniture.transform.position = position;

        return furniture;
    }
}
