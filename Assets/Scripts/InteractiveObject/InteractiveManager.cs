using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveManager : MonoBehaviour, IInteractable
{
    [SerializeField] private Outline outlineMesh;
    [SerializeField] private RectTransform[] inputClues;

    private bool outlineBlock;

    protected bool OutlineBlock
    {
        get => outlineBlock;
        set => outlineBlock = value;
    }

    public RectTransform[] GetInputClue()
    {
        return inputClues;
    }

    public abstract void Interact();

    public void OutlineEnabled(bool enabled)
    {
        if(outlineMesh != null && outlineBlock == false)
        {
            if (enabled != outlineMesh.enabled)
            {
                outlineMesh.enabled = enabled;
            }
        }
    }
}
