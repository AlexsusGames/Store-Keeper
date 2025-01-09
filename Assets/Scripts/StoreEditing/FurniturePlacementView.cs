using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePlacementView : MonoBehaviour
{
    [SerializeField] private string furnitureName;
    [SerializeField] private Outline outline;
    [SerializeField] private int collidersAmount;

    [SerializeField] private List<StorageSurface> surfaces;

    private BoxCollider bCollider;
    public string FurnitureName => furnitureName;
    public string FurnitureId { get; set; }

    private void Awake()
    {
        bCollider = GetComponent<BoxCollider>();
    }

    public void Select(bool value)
    {
        outline.enabled = value;
        ChangeOutlineColor(Color.white);
    }

    public bool HasProducts()
    {
        for (int i = 0; i < surfaces.Count; i++)
        {
            if (surfaces[i].transform.childCount != 0)
                return true;
        }

        return false;
    }

    public bool CheckAvailableToPlace()
    {
        Vector3 position = bCollider.bounds.center;
        Vector3 halfExtents = bCollider.bounds.size / 2f;

        Collider[] results = Physics.OverlapBox(position, halfExtents, Quaternion.identity);

        Debug.Log(results.Length);

        Color color = results.Length == collidersAmount ? Color.green : Color.red;
        ChangeOutlineColor(color);

        return results.Length == collidersAmount;
    }


    public void ChangeOutlineColor(Color color)
    {
        outline.OutlineColor = color;
    }
}
