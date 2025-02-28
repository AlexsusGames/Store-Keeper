using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private FirstPersonCamera playerCamera;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private InteractiveHandler interactiveHandler;

    [SerializeField] private Transform cartPoint;
    public Transform CartPoint => cartPoint;
    public FirstPersonCamera FirstPersonCamera => playerCamera;
    public InteractiveHandler InteractiveHandler => interactiveHandler;

    private void Awake()
    {
        playerController.TabletActivity = false;
    }

    public void BlockControl(bool value)
    {
        playerController.MovementBlockEnabled(value);
        FirstPersonCamera.SetCameraBlockEnabled(value);

        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
