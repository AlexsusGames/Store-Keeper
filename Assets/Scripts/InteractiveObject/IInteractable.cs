using UnityEngine;

public interface IInteractable 
{
    RectTransform[] GetInputClue();
    void Interact();

    void OutlineEnabled(bool enabled);
}
