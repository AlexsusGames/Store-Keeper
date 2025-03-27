using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class QuestCreator : MonoBehaviour
{
    [SerializeField] private QuestConfig[] questId;

    [Inject] private QuestConfigFinder configFinder;

    private QuestInteractor inteructor;

    private void Awake()
    {
        Core.Quest = this;
    }

    private void Start()
    {
        inteructor = Core.Interactors.GetInteractor<QuestInteractor>();
        inteructor.Bind(configFinder);
    }

    public void StartQuest(QuestType type)
    {
        string id = questId[(int)type].Id;

        inteructor.AddQuest(id);
    }

    public void TryChangeQuest(QuestType type, int value = 1)
    {
        string id = questId[(int)type].Id;

        inteructor.ChangeQuest(id, value);
    }
}
public enum QuestType
{
    OpenComputer,
    BuyStorage,
    PlaceStorage,
    OrderShipment,
    GrabClipboard,
    FinishSupply
}
