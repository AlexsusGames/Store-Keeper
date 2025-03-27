using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class QuestInteractor : Interactor
{
    private QuestConfigFinder configFinder;
    private QuestDataProvider dataProvider;
    private List<QuestData> quests => dataProvider.QuestData.ActiveQuests;

    public event Action<string> OnQuestAdded;
    public event Action<string> OnQuestCompleted;
    public event Action<string, int> OnQuestChanged;

    public override void Init()
    {
        dataProvider = Core.DataProviders.GetDataProvider<QuestDataProvider>();
    }

    public void Bind(QuestConfigFinder configFinder)
    {
        this.configFinder = configFinder;
    }

    public void UpdateView()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            OnQuestChanged?.Invoke(quests[i].Id, quests[i].CurrenProgress);
        }
    }

    public void AddQuest(string questId)
    {
        QuestData data = new QuestData()
        {
            Id = questId,
            CurrenProgress = 0,
        };

        quests.Add(data);

        OnQuestAdded?.Invoke(questId);
    }

    public void ChangeQuest(string questId, int value)
    {
        if (!HasQuest(questId))
            return;

        var quest = GetById(questId);

        var newValue = value + quest.CurrenProgress;

        quest.CurrenProgress = newValue;

        OnQuestChanged?.Invoke(questId, newValue);

        var config =  configFinder.FindById(questId);

        if (newValue >= config.AimAmount)
            CompleteQuest(questId);
    }

    private bool HasQuest(string questId) => GetById(questId) != null;

    private QuestData GetById(string questId)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (questId == quests[i].Id)
                return quests[i];
        }

        return null;
    }

    private void CompleteQuest(string id)
    {
        var quest = GetById(id);

        quests.Remove(quest);

        OnQuestCompleted?.Invoke(id);
    }

}
