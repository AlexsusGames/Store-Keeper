using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class BotDialogCreator 
{
    private string ContinueText = "Continue";
    private string ReplayText = "Load last save";

    private DialogConfig config;
    public DialogConfig GetLossesReport(float losses, float maxLosses)
    {
        List<string> list = new List<string>();

        list.Add("Calculating losses...");
        list.Add($"Losses: ${losses}");

        if (losses >= maxLosses)
            list.Add("Status: Significant loss.");
        else if (losses > 1)
            list.Add("Status: Minor loss.");
        else
            list.Add("Status: No loss detected.");

        return CreateDialog(list);
    }

    public DialogConfig GetDeliveryReport(Dictionary<string, float> report, bool wereSpoilt, string name, Sprite sprite)
    {
        bool wereMistakes = false;

        List<string> list = new List<string>()
        {
            "Hello, you recently sent goods to our store.",
        };

        foreach(var unit in report.Keys)
        {
            if (report[unit] < -0.1f)
            {
                if(wereMistakes == false)
                {
                    wereMistakes = true;
                    list.Add("I noticed that some products were missing.");
                    list.Add($"{unit} - {Math.Abs(report[unit])}");
                }
                else list.Add($"{unit} - {Math.Abs(report[unit])}");
            }
        }

        if (wereSpoilt)
        {
            list.Add("Some items were spoiled—this is unacceptable!");
            wereMistakes = true;
        }

        string lastMessage = wereMistakes ? "The payment will reflect that." : "The quantity and quality are just right. Thank you!";
        list.Add(lastMessage);

        var dialog = CreateDialog(list);

        dialog.CallerSprite = sprite;
        dialog.CallerName = name;

        dialog.FirstAction = ContinueText;
        dialog.SecondAction = ReplayText;

        return dialog;
    }

    private DialogConfig CreateDialog(List<string> list)
    {
        config = ScriptableObject.CreateInstance<DialogConfig>();

        config.Messages = new MessageConfig[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            config.Messages[i] = ScriptableObject.CreateInstance<MessageConfig>();
            config.Messages[i].IsPlayer = false;
            config.Messages[i].Content = new MessageContent() { Message = list[i] };

            config.Messages[i].TimeTillNextMessage = 1;
        }

        config.FirstAction = ContinueText;
        config.SecondAction = ReplayText;

        return config;
    }
}
