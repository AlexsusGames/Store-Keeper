using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Management : MonoBehaviour
{
    [SerializeField] private ProductManager productManager;
    [SerializeField] private GameEntryPoint gameEntryPoint;

    [SerializeField] private TMP_Text dayText;

    private void Start() => dayText.text = $"Day {Core.Statistic.GetDaysPassed() + 1}";

    public void StartNewDay()
    {
        Bank.OnDayEnd();

        productManager.TrySpoilProducts();

        gameEntryPoint.SaveData();

        Core.StartCamera = CameraType.GameplayCamera;

        SceneManager.LoadScene(0);
    }
}
