using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestInfoView : MonoBehaviour
{
    [SerializeField] private Image questImage;
    [SerializeField] private Image checkMarkImage;
    [SerializeField] private TMP_Text questName;
    [SerializeField] private TMP_Text questDescribtion;

    public void SetData(QuestConfig config, int currentValue = 0)
    {
        questImage.sprite = config.Icon;
        questName.text = config.Title;
        questDescribtion.text = config.Description;

        if(config.AimAmount > 1)
        {
            questDescribtion.text += $"\n[{currentValue}/{config.AimAmount}]";
        }
    }

    public void CompleteQuest(Action callback)
    {
        StartCoroutine(FadeAnimation(callback));
    }

    private IEnumerator FadeAnimation(Action callback)
    {
        List<Graphic> graphics = new List<Graphic>()
        {
            questName, questDescribtion
        };

        checkMarkImage.gameObject.SetActive(true);

        float elapsedTime = 0f;
        float fadeDuration = 1f;

        float startAlfa = questImage.color.a;

        Color[] colors = new Color[graphics.Count];

        for (int i = 0; i < graphics.Count; i++)
        {
            colors[i] = graphics[i].color;
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlfa, 0, elapsedTime / fadeDuration);

            for (int i = 0; i < graphics.Count; i++)
            {
                colors[i].a = alpha;
                graphics[i].color = colors[i];
            }

            yield return null;
        }

        for (int i = 0; i < graphics.Count; i++)
        {
            colors[i].a = 0;
            graphics[i].color = colors[i];
        }

        callback?.Invoke();
    }
}
