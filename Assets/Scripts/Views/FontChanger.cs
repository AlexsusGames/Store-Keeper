using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class FontChanger : MonoBehaviour
{
    [Inject] private Fonts fonts;
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();

        UpdateLocalization();

        Core.Localization.OnChanged += UpdateLocalization;
    }

    private void OnDestroy()
    {
        Core.Localization.OnChanged -= UpdateLocalization;
    }

    private void UpdateLocalization()
    {
        if(fonts == null) fonts = FindAnyObjectByType<Fonts>();

        text.font = fonts.GetByIndex(Core.Localization.LocalizationIndex);
    }
}
