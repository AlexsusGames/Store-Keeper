using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompanyTypeView : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private CompanyType type;

    [SerializeField] private Color trueColor;
    [SerializeField] private Color falseColor;

    public void UpdateView(List<CompanyType> types)
    {
        text.text = Core.Localization.Translate(type.ToString());

        text.color = types.Contains(type) ? trueColor : falseColor;
    }
}
