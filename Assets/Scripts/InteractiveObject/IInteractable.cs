using UnityEngine;

public interface IInteractable 
{
    InputViewConfig[] GetInputClue();
    void Interact();

    void OutlineEnabled(bool enabled);
}
