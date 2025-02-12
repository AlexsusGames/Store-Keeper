using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoxInfoView : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text countText;

    public void ShowInfo(StoreBox box)
    {
        gameObject.SetActive(true);

        nameText.text = box.ProductName;
        countText.text = $"{box.GetItemsAmount()}/{box.GetCapacity()}";
    }

    public void Hide() => gameObject.SetActive(false);
}
