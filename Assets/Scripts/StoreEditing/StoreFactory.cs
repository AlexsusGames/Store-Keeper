using Unity.VisualScripting;
using UnityEngine;

public class StoreFactory : MonoBehaviour 
{
    private StoreFurnitureConfigFinder configFinder;
    private Transform parent;

    public void Init(StoreFurnitureConfigFinder configFinder)
    {
        parent = transform;
        this.configFinder = configFinder;
    }

    public FurniturePlacementView Create(string id)
    {
        var prefab = configFinder.FindByName(id).prefab;

        var obj = Instantiate(prefab, parent);

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
