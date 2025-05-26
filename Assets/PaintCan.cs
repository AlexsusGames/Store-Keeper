using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintCan : InteractiveManager
{
    [SerializeField] private PaintManager manager;
    [SerializeField] private int index;

    public override void Interact()
    {
        manager.ChangeMaterial(index);
    }
}
