using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CarSlotView : MonoBehaviour
{
    [SerializeField] private TMP_Text carAmount;
    [SerializeField] private Image carImage;

    [SerializeField] private Sprite[] carSprites;
    public Sprite CarSprite { get; private set; }
    public CarType CarType { get; private set; }
    public bool Interactable
    {
        get => button.interactable;
        set
        {
            if(button != null)
            {
                button.interactable = value;
            }
        }
    }

    private Button button;
    public void SetData(int amount, CarType carType)
    {
        CarType = carType;
        CarSprite = carSprites[(int)carType];

        gameObject.SetActive(true);
        carImage.sprite = CarSprite;
        carAmount.text = amount.ToString();
    }

    public void AssignListener(UnityAction action)
    {
        if(button == null)
            button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    public void Hide() => gameObject.SetActive(false);
}
