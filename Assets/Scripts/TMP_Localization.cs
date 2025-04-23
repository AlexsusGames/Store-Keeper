using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class TMP_Localization : MonoBehaviour
{
    [SerializeField] private bool notChangeFont;
    [Inject] private Fonts fonts;
    private TMP_Text text;

    private string key;

    private void Start()
    {
        text = GetComponent<TMP_Text>();

        key = text.text;

        UpdateLocalization();

        Core.Localization.OnChanged += UpdateLocalization;
    }

    private void UpdateLocalization()
    {
        string localizated = Core.Localization.Translate(key);

        if(!notChangeFont)
            text.font = fonts.GetByIndex(Core.Localization.LocalizationIndex);

        text.text = localizated;
    }
}
