using Unity.VisualScripting;
using UnityEngine;

public class FurnitureFactory : MonoBehaviour 
{
    private StoreFurnitureConfigFinder configFinder;
    private Transform parent;

    public void Init(Transform transform, StoreFurnitureConfigFinder configFinder)
    {
        this.parent = transform;
        this.configFinder = configFinder;
    }

    public FurniturePlacementView Create(string id)
    {
        var prefab = configFinder.FindById(id);

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