using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBox : Box
{
    [SerializeField] private Transform childPoint;
    public bool IsHasChild => childPoint.childCount > 0;
    public StoreBox ChilBox => childPoint.GetComponent<StoreBox>();
    public Transform ChildPoint => childPoint;
}
