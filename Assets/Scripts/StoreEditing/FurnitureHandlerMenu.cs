using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class FurnitureHandlerMenu : MonoBehaviour
{
    [SerializeField] private Button storeButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button replaceButton;

    [SerializeField] private Image furnitureImage;

    [Inject] private StoreFurnitureConfigFinder configFinder;

    public void SetFurniture(string name, UnityAction storeAction, UnityAction confirmAction, UnityAction replaceAction)
    {
        furnitureImage.sprite = configFinder.FindByName(name).shopSprite;

        gameObject.SetActive(true);

        AssignButton(storeButton, storeAction);
        AssignButton(confirmButton, confirmAction);
        AssignButton(replaceButton, replaceAction);
    }

    public void ChangeConfirmButtonInterractable(bool value)
    {
        confirmButton.interactable = value;
    }

    private void AssignButton(Button button, UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    public void Hide() => gameObject.SetActive(false);
}
