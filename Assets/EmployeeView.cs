using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EmployeeView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text employeeName;
    [SerializeField] private TMP_Text salaryText;

    [SerializeField] private Color hiredColor;
    [SerializeField] private Color standartColor;

    public void SetData(EmployeeType type, int price ,bool isHired)
    {
        Color color = isHired ? hiredColor : standartColor;
        image.color = color;

        gameObject.SetActive(true);

        employeeName.text = Core.Localization.Translate(type.ToString());
        salaryText.text = $"${price}";
    }

    public void AssignListener(UnityAction action)
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();

        btn.onClick.AddListener(action);
    }

    public void Hide() => gameObject.SetActive(false);
}
