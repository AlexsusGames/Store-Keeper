using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    [SerializeField] private GameObject messengerObj;
    [SerializeField] private PhoneActionPanel actionPanel;
    [SerializeField] private Messanger messanger;

    [SerializeField] private Image callerImage;
    [SerializeField] private TMP_Text callerName;

    [SerializeField] private Sprite botSprite;
    private string botName = "CheckBot";

    private DialogConfig cachedConfig;

    private UnityAction firstAction;
    private UnityAction secondAction;

    public void OpenMessenger(DialogConfig config, UnityAction firt, UnityAction second)
    {
        ResetPhone();

        firstAction = firt;
        secondAction = second;

        cachedConfig = config;

        callerImage.sprite = config.CallerSprite == null
            ? botSprite
            : config.CallerSprite;

        callerName.text = string.IsNullOrEmpty(config.CallerName)
            ? botName
            : config.CallerName;

        gameObject.SetActive(true);
        StartCoroutine(Timer(config.Messages));
    }

    public void OpenMessenger()
    {
        gameObject.SetActive(true);

        actionPanel.AssignListeners(null, null);

        StartCoroutine(Open());
    }

    public void ResetPhone()
    {
        actionPanel.Hide();
        messanger.Clear();

        messengerObj.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator Timer(MessageConfig[] dialog)
    {
        yield return StartCoroutine(Open());

        messanger.StartDialog(dialog);
    }

    private IEnumerator Open()
    {
        yield return new WaitForSeconds(1);

        messengerObj.SetActive(true);
    }

    private void Awake()
    {
        messanger.Init(actionPanel);
        messanger.OnChatFinished += OpenActions;
    }
    private void OnDestroy() => messanger.OnChatFinished -= OpenActions;

    private void OpenActions()
    {
        actionPanel.SetActionsNames(cachedConfig.FirstAction, cachedConfig.SecondAction);
        actionPanel.AssignListeners(firstAction, secondAction);
    }
}
