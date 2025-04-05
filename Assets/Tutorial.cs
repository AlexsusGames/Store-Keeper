using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    private const string KEY = "TUTOR_COMPLETED";
    private const int LAST_STAGE_NUMBER = 6;

    [SerializeField] private ComputerButtonsController computerButtonsController;
    [SerializeField] private PhoneController phoneController;

    [SerializeField] private DialogConfig[] tutorDialogs;
    public bool IsCompleted => PlayerPrefs.GetInt(KEY) == LAST_STAGE_NUMBER;

    private bool canContinue;

    public void StartTutor()
    {
        var inteructor = Core.Interactors.GetInteractor<QuestInteractor>();

        inteructor.OnQuestCompleted += Continue;

        Action callback = () =>
        {
            inteructor.OnQuestCompleted -= Continue;
        };

        StartCoroutine(TutorialWay(callback));
    }

    private IEnumerator TutorialWay(Action callback)
    {
        int stageIndex = GetSavedIndex();
        
        if(stageIndex == 0)
        {
            AssignActions(QuestType.OpenComputer, 0);

            computerButtonsController.MakeTheOnlyActive(ComputerButton.StorageShop);

            yield return WaitForComplete();

            AssignActions(QuestType.BuyStorage, 1);

            yield return WaitForComplete();

            AssignActions(QuestType.PlaceStorage, 2);

            computerButtonsController.MakeTheOnlyActive(ComputerButton.StoreEdit);

            yield return WaitForComplete();

            AssignActions(QuestType.OrderShipment, 3);

            computerButtonsController.MakeTheOnlyActive(ComputerButton.Supplies);

            yield return WaitForComplete();

            AssignActions(QuestType.GrabClipboard, 4);

            yield return WaitForComplete();

            AssignActions(QuestType.FinishSupply, 5);

            yield return WaitForComplete();

            stageIndex++;

            SaveStage(stageIndex);
        }

        if(stageIndex > 0)
            computerButtonsController.ResetInteractable();

        if (stageIndex == 1)
        {
            var inteructor = Core.Interactors.GetInteractor<DayProgressInteractor>();

            Debug.Log(inteructor.GetCurrentLosses());

            var dialogIndex = inteructor.GetCurrentLosses() < 10 ? 6 : 7;

            AssignActions(QuestType.FinishSupply, dialogIndex);

            yield return WaitForComplete();
        }

        if (canContinue)
        {
            callback?.Invoke();
        }
    }

    private void SaveStage(int index)
    {
        PlayerPrefs.SetInt(KEY, index);
    }

    private int GetSavedIndex() => PlayerPrefs.GetInt(KEY);

    private void AssignActions(QuestType type, int dialogIndix)
    {
        canContinue = false;

        UnityAction firstAction = () =>
        {
            Core.Quest.StartQuest(type);
            phoneController.ClosePhone();
        };

        phoneController.OpenMessenger(tutorDialogs[dialogIndix], firstAction, null);
    }

    private IEnumerator WaitForComplete()
    {
        while (canContinue == false)
        {
            yield return null;
        }
    }

    private void Continue(string _) => canContinue = true;
}
