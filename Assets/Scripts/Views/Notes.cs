using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notes : MonoBehaviour
{
    [SerializeField] private TMP_Text notesText;

    private void OnEnable()
    {
        Core.Quest.TryChangeQuest(QuestType.GrabClipboard);
    }

    public void SetData(float value)
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        notesText.text = $"{value}.";
    }
}
