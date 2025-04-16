using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Management : MonoBehaviour
{
    [SerializeField] private ProductManager productManager;
    [SerializeField] private GameEntryPoint gameEntryPoint;

    [SerializeField] private DeliveringManager deliveryManager;

    [SerializeField] private TMP_Text dayText;

    private void OnEnable() => Core.Quest.TryChangeQuest(QuestType.CheckMail);
    private void Start() => dayText.text = $"Day {Core.Statistic.GetDaysPassed() + 1}";

    public void StartNewDay()
    {
        if (!deliveryManager.isCarArrived)
        {
            Core.Quest.TryChangeQuest(QuestType.EndDay);

            Bank.OnDayEnd();

            productManager.TrySpoilProducts();

            gameEntryPoint.SaveData();

            Core.StartCamera = CameraType.GameplayCamera;

            SceneManager.LoadScene(0);
        }
        else Core.Clues.Show("You cannot end the day while a truck is waiting to be loaded.");
    }
}
