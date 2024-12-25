using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePlacementView : MonoBehaviour
{
    [SerializeField] private string furnitureName;
    [SerializeField] private Outline outline;
    private BoxCollider bCollider;
    private bool isPossibleToPlace;
    public bool IsPossibleToPlace => isPossibleToPlace;
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

    public bool CheckAvailableToPlace()
    {
        Vector3 position = bCollider.bounds.center;
        Vector3 halfExtents = bCollider.bounds.size / 2f;

        Collider[] results = Physics.OverlapBox(position, halfExtents, Quaternion.identity);

        Color color = results.Length == 1 ? Color.green : Color.red;
        ChangeOutlineColor(color);

        return results.Length == 1;
    }


    public void ChangeOutlineColor(Color color)
    {
        outline.OutlineColor = color;
    }
}
