using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputUnitView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text action;

    private InputViewConfig cachedConfig;

    public void SetData(InputViewConfig config)
    {
        gameObject.SetActive(true);

        image.sprite = config.InputSprite;

        string translatedText = Core.Localization.Translate(config.InputName);

        action.text = translatedText;

        cachedConfig = config;
    }

    public void DisableViewByConfig(InputViewConfig config)
    {
        if(config == cachedConfig)
        {
            gameObject.SetActive(false);
        }
    }
}
