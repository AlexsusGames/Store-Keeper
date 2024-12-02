using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveManager : MonoBehaviour, IInteractable
{
    [SerializeField] private Outline outlineMesh;
    public abstract void Interact();

    public void OutlineEnabled(bool enabled)
    {
        outlineMesh.enabled = enabled;
    }
}
