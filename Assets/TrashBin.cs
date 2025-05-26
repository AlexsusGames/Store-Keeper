using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : InteractiveManager
{
    [SerializeField] private Animator trashAnim;
    [SerializeField] private ItemGrab itemGrab;

    public override void Interact()
    {
        itemGrab.GetRidOfEmptyBox();

        trashAnim.SetTrigger("use");
    }
}
