using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageImageView : MessageView
{
    [SerializeField] private Image image;

    public override void Show(MessageContent content)
    {
        SetTime();
        image.sprite = content.Sprite;
    }
}
