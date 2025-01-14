using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStorage : InteractiveManager
{
    [SerializeField] private ItemGrab itemGrab;
    public override void Interact()
    {
        itemGrab.GetRidOfEmptyBox();
    }
}
