using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StorageSurface : MonoBehaviour
{
    [SerializeField] private FurniturePlacementView parent;
    [SerializeField] private Door door;

    public bool IsOpened()
    {
        if (door == null)
            return true;

        return door.IsOpen;
    }
}
