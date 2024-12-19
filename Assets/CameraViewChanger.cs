using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewChanger : MonoBehaviour
{
    [SerializeField] private GameObject lockedCursorObj;
    [SerializeField] private Player player;
    [SerializeField] private TopDownCameraMovement topDownCamera;
    [SerializeField] private GameObject UiTopDownCamera;

    public bool isTopDownMode = false;

    private void Update()
    {
        if (isTopDownMode && Input.GetButtonDown("Cancel"))
        {
            SwitchTopDownCamera();
        }
    }

    public void SwitchTopDownCamera()
    {
        isTopDownMode = !isTopDownMode;
        topDownCamera.gameObject.SetActive(isTopDownMode);
        UiTopDownCamera.gameObject.SetActive(isTopDownMode);

        player.InteractiveHandler.ChangeEnavled(!isTopDownMode);
    }

    private void CursorEnabled(bool value)
    {
        lockedCursorObj.SetActive(!value);
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void MovePlayerCamera(Vector3 cords, Vector3 viewPoint)
    {
        player.PlayerController.MovementBlockEnabled(true);
        player.FirstPersonCamera.SetCameraBlockEnabled(true);
        player.FirstPersonCamera.SetRotation(viewPoint);
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
