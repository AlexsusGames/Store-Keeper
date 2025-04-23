using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSettings : MonoBehaviour
{
    [Header("Custom Settings")]

    public GameObject BoxObject;
    public string ProductName;
    public BoxType BoxType;
    public bool FoldByType;
    public bool UseWeigh;
    public bool IsFreeToPlace;

    [Space]

    [Header("Nullable Settings")]

    public Transform ChildPoint;
    public GameObject capObject;
    public GameObject spoilEffect;

    [Space]

    [Header("Static Settings")]

    [SerializeField] private Transform itemContainer;

    public Material BluePreviewMaterial;
    public Material RedPreviewMaterial;

    private GameObject[] items;
    private MeshRenderer[] renderers;

    public GameObject[] Products
    {
        get
        {
            if (items == null)
            {
                items = new GameObject[itemContainer.childCount];

                for (int i = 0; i < itemContainer.childCount; i++)
                {
                    items[i] = itemContainer.GetChild(i).gameObject;
                }
            }

            return items;
        }
    }

    public MeshRenderer[] Renderers
    {
        get
        {
            if (renderers == null)
            {
                List<MeshRenderer> list = new List<MeshRenderer>();

                for (int i = 0;i < Products.Length; i++)
                {
                    AddRenderer(Products[i], list);
                }

                if(capObject != null)
                {
                    AddRenderer(capObject, list);
                }

                AddRenderer(BoxObject, list);

                return list.ToArray();
            }

            return renderers;
        }
    }

    private void AddRenderer(GameObject gameObject, List<MeshRenderer> list)
    {
        if (gameObject.TryGetComponent(out MeshRenderer renderer))
        {
            list.Add(renderer);
        }
        else if(gameObject.transform.childCount > 0)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                AddRenderer(gameObject.transform.GetChild(i).gameObject, list);
            }
        }
        else throw new System.Exception($"Object: {gameObject.name} doesn't consist renderer");
    }

    private BoxCollider collisionCollider;

    public BoxCollider CollisionCollider
    {
        get
        {
            if(collisionCollider == null)
            {
                if(BoxObject.TryGetComponent(out BoxCollider boxCollider))
                {
                    collisionCollider = boxCollider;
                }
                else throw new System.Exception($"Object: {BoxObject.name} doesn't consist collider");
            }

            return collisionCollider;
        }
    }

    private BoxCollider surfaceCollider;

    public BoxCollider SurfaceCollider
    {
        get
        {
            if (ChildPoint == null)
                return null;

            if (surfaceCollider == null)
            {
                if (ChildPoint.TryGetComponent(out BoxCollider boxCollider))
                {
                    surfaceCollider = boxCollider;
                }
                else throw new System.Exception($"Object: {ChildPoint.name} doesn't consist collider");
            }

            return surfaceCollider;
        }
    }
}
