using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProductView : MonoBehaviour
{
    [SerializeField] private TMP_Text productName;
    [SerializeField] private TMP_Text productAmount;

    private Button button;

    public void SetData(string name, string amount, bool showAmount)
    {
        productName.text = name;

        productAmount.text = showAmount ? amount : "???";
    }

    public void AssignAction(UnityAction action)
    {
        if(button ==  null)
            button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
}
