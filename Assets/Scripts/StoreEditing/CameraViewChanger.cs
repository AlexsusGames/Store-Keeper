using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewChanger : MonoBehaviour, IWindow
{
    [SerializeField] private Player player;

    public bool cameraSwitched;
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
        cameraSwitched = true;
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
            cameraSwitched = false;
        };

        player.FirstPersonCamera.ResetCameraPosition(callback);
    }

    public void Close()
    {
        if (storeEditorModeEnabled)
        {
            SwitchTopDownCamera();
            return;
        }

        if (cameraSwitched)
        {
            cameraSwitched = false;
            ResetCameraPosition();
        }
    }

    public bool IsActive()
    {
        return storeEditorModeEnabled || cameraSwitched;
    }
}
