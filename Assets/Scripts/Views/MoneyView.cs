using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyView : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    private void Awake() => Bank.OnChanged += UpdateView;

    private void Start() => UpdateView();
    private void OnDestroy() => Bank.OnChanged -= UpdateView;

    private void UpdateView()
    {
        moneyText.text = $"${MathF.Round(Bank.MoneyAmount, 2)}";
    }
}
