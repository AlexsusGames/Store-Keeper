using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewChanger : MonoBehaviour, IWindow
{
    [SerializeField] private GameObject gameplayCanvas;
    [SerializeField] private Player player;
    [SerializeField] private GameObject storeEditor;
    [SerializeField] private Canvas computerUI;

    public bool storeEditorModeEnabled = false;

    public void SwitchTopDownCamera()
    {
        storeEditorModeEnabled = !storeEditorModeEnabled;
        storeEditor.gameObject.SetActive(storeEditorModeEnabled);

        player.InteractiveHandler.ChangeEnabled(!storeEditorModeEnabled);
        computerUI.enabled = !storeEditorModeEnabled;
    }

    private void UIEnabled(bool value)
    {
        gameplayCanvas.SetActive(!value);
    }

    public bool IsWorking => player.FirstPersonCamera.IsWorking;

    public void MovePlayerCamera(Vector3 cords, Vector3 viewPoint)
    {
        player.BlockControl(true);
        UIEnabled(true);

        player.FirstPersonCamera.SetNewCameraPosition(cords, null);
    }

    public void ResetCameraPosition()
    {
        Action callback = () =>
        {
            UIEnabled(false);
            player.BlockControl(false);
        };

        player.FirstPersonCamera.ResetCameraPosition(callback);
    }

    public void Close()
    {
        if (storeEditorModeEnabled)
        {
            SwitchTopDownCamera();
        }
    }
}
