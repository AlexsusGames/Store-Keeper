using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CluesPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text clueText;
    [SerializeField] private Graphic[] uiElements;
    [SerializeField] private float animDuration;

    public void ShowClue(string text)
    {
        gameObject.SetActive(true);

        clueText.text = text;
        StartCoroutine(Animation(animDuration));
    }

    private IEnumerator Animation(float duration)
    {
        yield return StartCoroutine(FadeUIElements(0, 1, duration));

        yield return new WaitForSeconds(3);

        yield return StartCoroutine(FadeUIElements(1, 0, duration / 2));

        gameObject.SetActive(false);
    }

    private IEnumerator FadeUIElements(float currentAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        float[] startAlphas = new float[uiElements.Length];

        for (int i = 0; i < uiElements.Length; i++)
        {
            startAlphas[i] = currentAlpha;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            for (int i = 0; i < uiElements.Length; i++)
            {
                Color newColor = uiElements[i].color;
                newColor.a = Mathf.Lerp(startAlphas[i], targetAlpha, t);
                uiElements[i].color = newColor;
            }

            yield return null; 
        }

        for (int i = 0; i < uiElements.Length; i++)
        {
            Color newColor = uiElements[i].color;
            newColor.a = targetAlpha;
            uiElements[i].color = newColor;
        }
    }
}
