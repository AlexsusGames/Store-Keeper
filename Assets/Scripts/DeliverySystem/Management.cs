using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Management : MonoBehaviour
{
    [SerializeField] private ProductManager productManager;
    [SerializeField] private GameEntryPoint gameEntryPoint;

    [SerializeField] private DeliveringManager deliveryManager;

    [SerializeField] private TMP_Text dayText;

    private void OnEnable()
    {
        Core.Quest.TryChangeQuest(QuestType.CheckMail);
    }
    private void Start() => dayText.text = $"{Core.Localization.Translate("Day")} {Core.Statistic.GetDaysPassed() + 1}";

    public void StartNewDay()
    {
        if (!deliveryManager.isCarArrived)
        {
            Core.Quest.TryChangeQuest(QuestType.EndDay);

            Action callback = () =>
            {
                Core.Interactors.GetInteractor<ShopInteractor>().OnDayEnd();

                Bank.OnDayEnd();

                productManager.TrySpoilProducts();

                gameEntryPoint.SaveData();

                Core.StartCamera = CameraType.GameplayCamera;

                SceneManager.LoadScene(0);
            };

            FadeScreen.instance.TryShowLoadingScreen(callback);
        }
        else Core.Clues.Show("You cannot end the day while a truck is waiting to be loaded.");
    }
}
