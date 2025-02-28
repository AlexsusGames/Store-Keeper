using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LossesCounter : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text currentLosses;
    [SerializeField] private TMP_Text maxLosses;

    public void UpdateData(float currentLosses, float maxLosses)
    {
        this.currentLosses.text = $"{currentLosses}$";
        this.maxLosses.text = $"{maxLosses}$";

        float fillAmount = currentLosses / maxLosses;
        fillImage.fillAmount = fillAmount;
    }
}
