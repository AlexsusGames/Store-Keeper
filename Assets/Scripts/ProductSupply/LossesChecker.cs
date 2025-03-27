using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LossesChecker : MonoBehaviour
{
    [SerializeField] private TMP_Text[] log;

    public event Action<string, float> OnChecked;

    private const string RED_COLOR = "<color=red>";
    private const string GREEN_COLOR = "<color=green>";

    private List<string> messages;

    public void Check(Dictionary<string, float> expected, Dictionary<string, float> noted, Action callBack)
    {
        gameObject.SetActive(true);
        messages = new();
        UpdateView();

        StartCoroutine(Timer(expected, noted, callBack));
    }
    public void Hide() => gameObject.SetActive(false);

    private IEnumerator Timer(Dictionary<string, float> actual, Dictionary<string, float> noted, Action callBack)
    {
        foreach(var product in noted.Keys)
        {
            string result = actual[product] == noted[product] ? $"{GREEN_COLOR}Successful" : $"{RED_COLOR}Failure";
            string newItem = $"{product} {noted[product]} / {actual[product]} - {result}";

            float different = actual[product] - noted[product];
            OnChecked?.Invoke(product, different);

            messages.Insert(0, newItem);
            UpdateView();

            yield return new WaitForSeconds(0.5f);
        }

        callBack?.Invoke();

        yield return new WaitForSeconds(2f);

        Hide();
    }

    private void UpdateView()
    {
        for (int i = 0; i < log.Length; i++)
        {
            if (i >= messages.Count)
                log[i].text = "";

            else log[i].text = messages[i];
        }
    }
}
