using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OrderButton : MonoBehaviour
{
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closeSprite;
    [SerializeField] private Sprite toggleSprite;

    private Image image;
    private Button button;
    public bool Enabled { get => gameObject.activeInHierarchy; set { gameObject.SetActive(value); } }
    public void CheckProductAvailability(DeliveryData data)
    {
        bool hasProducts = true;

        var list = data.OrderedProducts;

        for (int i = 0; i < list.Count; i++)
        {
            if (!Core.ProductList.Has(list[i].Product, list[i].Amount))
            {
                hasProducts = false;
                break;
            }
        }

        if (image == null)
        {
            image = GetComponent<Image>();
        }

        image.sprite = hasProducts ? toggleSprite : closeSprite;
    }
    public void AssignAction(UnityAction action)
    {
        action += MarkOpened;

        if(button == null)
            button = gameObject.GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    private void MarkOpened()
    {
        if(image == null)
        {
            image = GetComponent<Image>();
        }

        image.sprite = openSprite;
    }
}
