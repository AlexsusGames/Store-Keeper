using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveManager
{
    [SerializeField] private Animator animator;

    private bool isOpen;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public override void Interact()
    {
        isOpen = !isOpen;

        animator.SetBool(nameof(isOpen), isOpen);

        if (isOpen) Core.Sound.PlayClip(AudioType.Door);
        else Core.Sound.PlayClip(AudioType.DoorClose);
    }

    public bool IsOpen => isOpen;
}
