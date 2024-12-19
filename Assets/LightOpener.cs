using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOpener : InteractiveManager
{
    [SerializeField] private Animator animator;
    private bool isEnabled;

    public override void Interact()
    {
        isEnabled = !isEnabled;

        animator.SetBool(nameof(isEnabled), isEnabled);
    }
}
