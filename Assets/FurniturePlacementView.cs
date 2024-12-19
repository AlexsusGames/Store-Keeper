using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePlacementView : MonoBehaviour
{
    [SerializeField] private string furnitureId;
    [SerializeField] private Outline outline;
    private bool isPossibleToPlace;
    public bool IsPossibleToPlace => isPossibleToPlace;
    public string FurnitureId => furnitureId;

    public void Choose(bool value)
    {
        outline.enabled = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        outline.OutlineColor = Color.red;
        isPossibleToPlace = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        outline.OutlineColor = Color.green;
        isPossibleToPlace = true;
    }
}
