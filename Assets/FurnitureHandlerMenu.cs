using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FurnitureHandlerMenu : MonoBehaviour
{
    [SerializeField] private Button storeButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button replaceButton;

    [SerializeField] private Image furnitureImage;

    public void SetFurniture(Sprite sprite, UnityAction storeAction, UnityAction confirmAction, UnityAction replaceAction)
    {
        furnitureImage.sprite = sprite;

        gameObject.SetActive(true);

        AssignButton(storeButton, storeAction);
        AssignButton(confirmButton, confirmAction);
        AssignButton(replaceButton, replaceAction);
    }

    private void AssignButton(Button button, UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
        button.onClick.AddListener(Hide);
    }

    public void Hide() => gameObject.SetActive(false);
}
