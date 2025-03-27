using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class QuestView : MonoBehaviour
{
    [SerializeField] private RectTransform[] positions;
    [SerializeField] private RectTransform questPrefab;

    [Inject] private QuestConfigFinder configFinder;

    private Dictionary<string, RectTransform> questMap = new();

    private void Start()
    {
        var interactor = Core.Interactors.GetInteractor<QuestInteractor>();

        interactor.OnQuestAdded += StartQuest;
        interactor.OnQuestChanged += ChangeQuest;
        interactor.OnQuestCompleted += CompleteQuest;

        interactor.UpdateView();
    }

    private void StartQuest(string questId)
    {
        var position = GetFreePosition();

        var questView = Instantiate(questPrefab, position);
        questView.TryGetComponent(out QuestInfoView view);

        UpdateQuest(view, questId);

        questView.anchoredPosition = Vector2.zero;

        questMap[questId] = questView;
    }

    private void ChangeQuest(string questId, int currentValue)
    {
        if (!questMap.ContainsKey(questId))
        {
            StartQuest(questId);
        }

        var quest = questMap[questId];

        quest.TryGetComponent(out QuestInfoView view);

        UpdateQuest(view, questId, currentValue);
    }

    private void CompleteQuest(string questId)
    {
        var quest = questMap[questId];

        questMap.Remove(questId);

        quest.TryGetComponent(out QuestInfoView view);

        Action callback = () =>
        {
            Rebuild();
            Destroy(quest.gameObject);
        };

        view.CompleteQuest(callback);
    }

    private void UpdateQuest(QuestInfoView view, string id, int currentAmout = 0)
    {
        var quest = configFinder.FindById(id);

        view.SetData(quest, currentAmout);
    }

    private void Rebuild()
    {
        int index = 0;

        foreach (var item in questMap.Values)
        {
            var freePosition = positions[index];

            index++;

            item.parent = freePosition;
            item.anchoredPosition = Vector2.zero;
        }
    }

    private RectTransform GetFreePosition()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            if (positions[i].childCount == 0)
                return positions[i];
        }

        throw new System.Exception("There is no free transform for new quest");
    }
}
