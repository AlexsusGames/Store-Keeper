using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class MessageView : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private RectTransform background;

    public float MessageHeigh => background.rect.height;

    protected void SetTime()
    {
        string formattedTime = DateTime.Now.ToString("HH:mm");
        timeText.text = formattedTime;
    }

    public abstract void Show(MessageContent content);
}
