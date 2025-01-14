using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveManager : MonoBehaviour, IInteractable
{
    [SerializeField] private Outline outlineMesh;
    [SerializeField] private RectTransform[] inputClues;

    public RectTransform[] GetInputClue()
    {
        return inputClues;
    }

    public abstract void Interact();

    public void OutlineEnabled(bool enabled)
    {
        if(enabled != outlineMesh.enabled)
        {
            outlineMesh.enabled = enabled;
        }
    }
}
