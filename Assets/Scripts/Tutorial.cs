using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private const string KEY = "TUTOR_COMPLETED";
    private const int LAST_STAGE_NUMBER = 6;

    [SerializeField] private bool isDemo;

    [SerializeField] private ComputerButtonsController computerButtonsController;
    [SerializeField] private GameObject demoWindow;
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private SupplyConditions supplyConditions;

    [SerializeField] private DialogConfig[] tutorDialogs;
    [SerializeField] private Button signoutBtn;

    private bool canContinue;

    public void StartQuestLine()
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
        {
            computerButtonsController.ResetInteractable();
        }

        if (stageIndex == 1)
        {
            signoutBtn.interactable = false;

            var losses = Core.Statistic.GetTotalLosses();

            var dialogIndex = losses < 0 ? 6 : 7;

            AssignActions(QuestType.FinishSupply, dialogIndex);

            yield return WaitForComplete();

            stageIndex++;

            SaveStage(stageIndex);

            signoutBtn.interactable = true;
        }

        if(stageIndex == 2)
        {
            computerButtonsController.MakeTheOnlyBlocked(ComputerButton.Goods);

            AssignActions(QuestType.EndDay, 8);

            yield return WaitForComplete();

            stageIndex++;

            SaveStage(stageIndex);
        }

        if(stageIndex == 3)
        {
            computerButtonsController.ResetInteractable();

            signoutBtn.interactable = false;

            AssignActions(QuestType.CheckMail, 9);

            yield return WaitForComplete();

            AssignActions(QuestType.FinishDelivery, 10);

            yield return WaitForComplete();

            Action action = () =>
            {
                canContinue = true;
            };

            AssignActions(QuestType.EarnMoney, 11, action);

            yield return WaitForComplete();

            yield return new WaitForSeconds(2);

            UnityAction firstAction = () =>
            {
                phoneController.ClosePhone();
            };

            phoneController.OpenMessenger(tutorDialogs[12], firstAction, null);

            canContinue = false;

            stageIndex++;

            SaveStage(stageIndex);

            signoutBtn.interactable = true;
        }

        supplyConditions.UnlockSystems(new int[] { 0, 1, 2, 3 });

        if (stageIndex == 4)
        {
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

    private void AssignActions(QuestType type, int dialogIndix, Action callback = null)
    {
        canContinue = false;

        UnityAction firstAction = () =>
        {
            Core.Quest.StartQuest(type);
            phoneController.ClosePhone();

            callback?.Invoke();
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
