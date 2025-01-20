using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewChanger : MonoBehaviour
{
    [SerializeField] private GameObject gameplayCanvas;
    [SerializeField] private Player player;
    [SerializeField] private GameObject storeEditor;
    [SerializeField] private Canvas computerUI;

    public bool storeEditorModeEnabled = false;

    private void Update()
    {
        if (storeEditorModeEnabled && Input.GetButtonDown("Cancel"))
        {
            SwitchTopDownCamera();
        }
    }

    public void SwitchTopDownCamera()
    {
        storeEditorModeEnabled = !storeEditorModeEnabled;
        storeEditor.gameObject.SetActive(storeEditorModeEnabled);

        player.InteractiveHandler.ChangeEnabled(!storeEditorModeEnabled);
        computerUI.enabled = !storeEditorModeEnabled;
    }

    private void CursorEnabled(bool value)
    {
        gameplayCanvas.SetActive(!value);
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public bool IsWorking => player.FirstPersonCamera.IsWorking;

    public void MovePlayerCamera(Vector3 cords, Vector3 viewPoint)
    {
        player.PlayerController.MovementBlockEnabled(true);
        player.FirstPersonCamera.SetCameraBlockEnabled(true);
        CursorEnabled(true);

        player.FirstPersonCamera.SetNewCameraPosition(cords, null);
    }

    public void ResetCameraPosition()
    {
        Action callback = () =>
        {
            CursorEnabled(false);
            player.PlayerController.MovementBlockEnabled(false);
            player.FirstPersonCamera.SetCameraBlockEnabled(false);
        };

        player.FirstPersonCamera.ResetCameraPosition(callback);
    }
}
