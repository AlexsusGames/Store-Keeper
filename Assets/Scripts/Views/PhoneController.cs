using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PhoneController : MonoBehaviour, IWindow
{
    [SerializeField] private Player player;
    [SerializeField] private Phone phone;

    [SerializeField] private DialogConfig test;
    public bool IsCanBeSkipped {  get; private set; }
    public void Replay()
    {
        Core.StartCamera = CameraType.GameplayCamera;
        SceneManager.LoadScene(0);
    }

    public void GoToMenu()
    {
        Core.StartCamera = CameraType.MainMenuCamera;
        SceneManager.LoadScene(0);
    }

    public void ClosePhone()
    {
        IsCanBeSkipped = true;

        Close();
    }

    public void OpenMessenger()
    {
        ChangePhoneEnabled(true, true);

        phone.OpenMessenger();
    }

    public void OpenMessenger(DialogConfig config)
    {
        ChangePhoneEnabled(true, false);

        UnityAction first = () => ChangePhoneEnabled(false, false);

        phone.OpenMessenger(config, first, null);
    }

    public void OpenMessenger(DialogConfig config, UnityAction first, UnityAction second)
    {
        ChangePhoneEnabled(true, false);
        phone.OpenMessenger(config, first, second);
    }

    public void OnGameOver()
    {
        UnityAction first = Replay;
        UnityAction second = GoToMenu;

        OpenMessenger(test, first, second);
    }

    public void Close()
    {
        if(IsCanBeSkipped)
        {
            ChangePhoneEnabled(false, false);
        }
    }

    public bool IsActive()
    {
        if (IsCanBeSkipped)
        {
            return gameObject.activeInHierarchy;
        }

        return false;
    }

    private void ChangePhoneEnabled(bool enabled, bool isCanBeSkipped)
    {
        BlockPlayer(enabled);
        this.IsCanBeSkipped = isCanBeSkipped;
        gameObject.SetActive(enabled);
    }

    private void BlockPlayer(bool value)
    {
        player.BlockControl(value, this);
        player.BlockInteractivity(value, this);
    }
}
