using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBox : Box
{
    [SerializeField] private Transform childPoint;
    [SerializeField] private bool useWeight;

    public BoxType BoxType;
    public bool FoldBtType;
    public bool UseWeight => useWeight;
    public bool IsHasChild
    {
        get
        {
            if(childPoint != null)
            {
                return childPoint.childCount > 0;
            }

            return false;
        }
    }

    public StoreBox ChilBox
    {
        get
        {
            if(IsHasChild) return childPoint.GetComponentInChildren<StoreBox>();

            return null;
        }
    }

    public float GetWeight() => GetItemsAmount() * ProductWeight;
    public Transform ChildPoint => childPoint;
}
public enum BoxType
{
    YellowBox = 0,
    BlackBox = 1,
    CartonBox = 2,
    CartonPlane = 3,
    CartonOpened = 4,
    Other
}
