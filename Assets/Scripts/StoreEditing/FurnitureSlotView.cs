using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureSlotView : MonoBehaviour
{
    [SerializeField] private TMP_Text textCount;
    [SerializeField] private Image furnitureImage;


    private bool isActive;
    public string ItemId;

    public void SetData(Sprite sprite, int count)
    {
        textCount.text = count.ToString();
        furnitureImage.sprite = sprite;

        textCount.enabled = true;
        furnitureImage.enabled = true;
        isActive = true;
    }

    public bool Enabled => isActive;

    public void Hide()
    {
        furnitureImage.enabled = false;
        textCount.enabled = false;

        isActive = false;
    }
}
