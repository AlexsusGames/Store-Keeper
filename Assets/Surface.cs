using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    [SerializeField] private List<Transform> surfaces;
    [SerializeField] private FurniturePlacementView view;
    [SerializeField] private string surfaceId = string.Empty;

    public List<Transform> Surfaces => surfaces;
    public string GetSurfaceId()
    {
        if (view != null)
            return view.FurnitureId;

        else return surfaceId;
    }
}
