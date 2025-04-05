using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BotDialogCreator 
{
    private string isCalculating = "Calculating losses...";
    private string noLossStatus = "Status: No loss detected.";
    private string significantLossStatus = "Status: Significant loss.";
    private string minorLossStatus = "Status: Minor loss.";

    private string ContinueText = "Continue";
    private string ReplayText = "Replay last shipment";

    private DialogConfig config;
    public DialogConfig GetLossesReport(float losses, float maxLosses)
    {
        List<string> list = new List<string>();

        list.Add(isCalculating);
        list.Add($"Losses: ${losses}");

        if (losses >= maxLosses)
            list.Add(significantLossStatus);
        else if (losses > 1)
            list.Add(minorLossStatus);
        else
            list.Add(noLossStatus);

        return CreateDialog(list);
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
        }

        config.FirstAction = ContinueText;
        config.SecondAction = ReplayText;

        return config;
    }
}
