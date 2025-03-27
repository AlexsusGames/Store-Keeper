using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerButtonsController : MonoBehaviour
{
    [SerializeField] private Button[] buttons;

    public void MakeTheOnlyActive(ComputerButton type)
    {
        int index = (int)type; 

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = i == index;
        }
    }

    public void ResetInteractable()
    {
        for (int i = 0;i < buttons.Length;i++)
        {
            buttons[i].interactable = true;
        }
    }
}
public enum ComputerButton
{
    StoreEdit,
    StorageShop,
    Supplies
}
