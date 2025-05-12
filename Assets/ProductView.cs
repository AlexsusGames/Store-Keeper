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

    public void SetData(string name, string amount)
    {
        productName.text = name;
        productAmount.text = amount;
    }

    public void AssignAction(UnityAction action)
    {
        if(button ==  null)
            button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
}
