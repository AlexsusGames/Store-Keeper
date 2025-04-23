using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageTextView : MessageView
{
    [SerializeField] private TMP_Text messageText;

    public override void Show(MessageContent content)
    {
        string translated = Core.Localization.Translate(content.Message);

        SetTime();
        messageText.text = translated;
    }
}
