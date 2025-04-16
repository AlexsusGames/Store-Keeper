using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Messanger : MonoBehaviour
{
    [SerializeField] private RectTransform messagePrefab;
    [SerializeField] private RectTransform playerMessagePrefab;
    [SerializeField] private RectTransform imageMessage;
    [SerializeField] private RectTransform content;

    [SerializeField] private float xPlayerOffset;
    [SerializeField] private float xCallerOffset;

    [SerializeField] private bool isTested;

    private float currentContentHeight;
    public event Action OnChatFinished;

    private List<RectTransform> messageList = new();

    private ScrollRect scroll;
    private PhoneActionPanel actionPanel;

    private bool isRead;

    public void Init(PhoneActionPanel panel)
    {
        this.actionPanel = panel;
    }

    private void Awake() => scroll = GetComponent<ScrollRect>();

    public void StartDialog(MessageConfig[] messages) => StartCoroutine(MessageSender(messages));

    public void Clear()
    {
        for (int i = 0; i < messageList.Count; ++i)
        {
            Destroy(messageList[i].gameObject);
        }

        messageList.Clear();
        currentContentHeight = 0;

        content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentContentHeight);
    }

    private IEnumerator MessageSender(MessageConfig[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
            if (isTested) continue;

            isRead = false;
            var message = messages[i];

            if (message.IsPlayer)
            {
                UnityAction action = () =>
                {
                    actionPanel.Hide();
                    Core.Sound.PlayClip(AudioType.MouseClick);
                    isRead = true;
                };

                actionPanel.AssignOneAction(action, message.Content.Message);

                while (!isRead)
                {
                    yield return null;
                }
            }

            SendMessage(message.IsPlayer, message.Content);

            Core.Sound.PlayClip(AudioType.ButtonClick);

            yield return new WaitForSeconds(message.TimeTillNextMessage);
        }
        OnChatFinished?.Invoke();
    }

    private void SendMessage(bool isPlayer, MessageContent msg)
    {
        float yStartPosition = messageList.Count == 0 
            ? 0 
            : messageList[messageList.Count - 1].anchoredPosition.y;

        RectTransform msgPrefab = isPlayer
            ? playerMessagePrefab
            : (msg.Sprite == null ? messagePrefab : imageMessage);


        var message = Instantiate(msgPrefab, content);

        if(message.TryGetComponent(out MessageView messageView))
        {
            messageView.Show(msg);

            Canvas.ForceUpdateCanvases();

            messageList.Add(message);

            float yPosition = -messageView.MessageHeigh + yStartPosition;
            float xPosition = isPlayer ? xPlayerOffset : xCallerOffset;

            float newSize = currentContentHeight - messageView.MessageHeigh;

            StartCoroutine(ScrollToBottomSmooth(newSize));

            currentContentHeight -= messageView.MessageHeigh;

            message.anchoredPosition = new Vector2(xPosition, yPosition);
        }

        else throw new System.Exception($"Object: {message.name} doesn't contain {nameof(MessageView)}");
    }

    private IEnumerator ScrollToBottomSmooth(float newHeigh)
    {
        float scrollSpeed = 0.3f;
        float currentHeigh = currentContentHeight;
        float elapsedTime = 0f;

        while (elapsedTime < scrollSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / scrollSpeed);
            float heigh = Mathf.Lerp(currentHeigh, newHeigh, t);

            content.sizeDelta = new Vector2(content.sizeDelta.x, heigh);
            scroll.verticalNormalizedPosition = 0;

            yield return null;
        }

        content.sizeDelta = new Vector2(content.sizeDelta.x, newHeigh);
    }
}
