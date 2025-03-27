using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewChanger : MonoBehaviour, IWindow
{
    [SerializeField] private Player player;

    public bool storeEditorModeEnabled = false;

    public void SwitchTopDownCamera()
    {
        storeEditorModeEnabled = !storeEditorModeEnabled;

        CameraType type = storeEditorModeEnabled
            ? CameraType.StoreEditorCamera
            : CameraType.GameplayCamera;

        Core.Camera.SetCurrentCamera(type);
    }

    public bool IsWorking => player.FirstPersonCamera.IsWorking;

    public void MovePlayerCamera(Vector3 cords, Vector3 viewPoint)
    {
        player.BlockControl(true, this);
        player.IsCameraPositionChanged = true;

        player.FirstPersonCamera.SetNewCameraPosition(cords, null);
    }

    public void ResetCameraPosition()
    {
        Action callback = () =>
        {
            player.IsCameraPositionChanged = false;
            player.BlockControl(false, this);
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

    public bool IsActive()
    {
        return storeEditorModeEnabled;
    }
}
