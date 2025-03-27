using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainMenuButtons : MonoBehaviour
{
    private const string NEW_GAME_KEY = "NEW_GAME_STARTED";

    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject confirmPanel;

    [Inject] private SettingsDataProvider settingsDataProvider;

    public bool newGameStarted => PlayerPrefs.HasKey(NEW_GAME_KEY);

    private void OnEnable()
    {
        UnityAction newGameAction = newGameStarted ? OpenConfirmPanel : StartNewGame;
        AssignListener(newGameButton, newGameAction);

        continueButton.interactable = PlayerPrefs.HasKey(NEW_GAME_KEY) ? true : false;
        AssignListener(continueButton, Continue);

        AssignListener(settingsButton, ChangeSettingsEnabled);
        AssignListener(quitButton, Exit);
    }

    public void Exit() => Application.Quit();

    public void StartNewGame()
    {
        var currentSettings = settingsDataProvider.GetData();

        PlayerPrefs.DeleteAll();

        settingsDataProvider.SetData(currentSettings);

        Core.StartCamera = CameraType.GameplayCamera;
        SceneManager.LoadScene(0);

        PlayerPrefs.SetInt(NEW_GAME_KEY, 1);
    }

    public void OpenConfirmPanel() => confirmPanel.SetActive(true);

    public void Continue() => Core.Camera.BackToLastState();

    public void ChangeSettingsEnabled()
    {
        bool enabled = settingsPanel.activeInHierarchy;

        enabled = !enabled;

        settingsPanel.SetActive(enabled);
    }

    private void AssignListener(Button button, UnityAction action)
    {
        button.onClick.RemoveAllListeners();

        action += () => Core.Sound.PlayClip(AudioType.MouseClick);

        button.onClick.AddListener(action);
    }
}
