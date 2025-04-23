using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBox : Box
{
    public BoxType BoxType => settings.BoxType;
    public bool FoldBtType => settings.FoldByType;
    public bool UseWeight => settings.UseWeigh;
    public bool IsHasChild
    {
        get
        {
            if(ChildPoint != null)
            {
                return ChildPoint.childCount > 0;
            }

            return false;
        }
    }

    public float GetWeight() => GetItemsAmount() * ProductWeight;
    public Transform ChildPoint => settings.ChildPoint;
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
