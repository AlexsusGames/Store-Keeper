using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rokla : InteractiveManager
{
    [SerializeField] private DeliveringManager delivetingManager;
    public override void Interact()
    {
        if (delivetingManager.SwapObjectsPositions())
        {
            PlayAnimation();
        }
    }

    private void PlayAnimation()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }
}
