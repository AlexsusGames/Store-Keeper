using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhoneActionPanel : MonoBehaviour
{
    [SerializeField] private Button firstActionButton;
    [SerializeField] private Button secondActionButton;

    [SerializeField] private Image standartImage;

    public void AssignListeners(UnityAction firstAction, UnityAction secondAction)
    {
        gameObject.SetActive(true);

        standartImage.gameObject.SetActive(firstAction == null && secondAction == null);

        if (firstAction != null)
        {
            AssignButton(firstActionButton, firstAction);
        }
        else firstActionButton.gameObject.SetActive(false);

        if(secondAction != null)
        {
            AssignButton(secondActionButton, secondAction);
        }
        else secondActionButton.gameObject.SetActive(false);
    }

    public void SetActionsNames(string firstAction, string secondAction)
    {
        firstActionButton.GetComponentInChildren<TMP_Text>().text = firstAction;
        secondActionButton.GetComponentInChildren<TMP_Text>().text= secondAction;
    }

    public void Hide() => gameObject.SetActive(false);

    public void AssignOneAction(UnityAction action, string name)
    {
        AssignListeners(action, null);
        SetActionsNames(name, "");
    }

    private void AssignButton(Button button, UnityAction action)
    {
        button.gameObject.SetActive(true);
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(action);
        button.onClick.AddListener(() => Core.Sound.PlayClip(AudioType.MouseClick));
    }

}
