using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveManager
{
    private Animator animator;
    private bool isOpen;

    private void Awake() => animator = GetComponent<Animator>();

    public override void Interact()
    {
        isOpen = !isOpen;

        animator.SetBool(nameof(isOpen), isOpen);
    }
}
