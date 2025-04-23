using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private FirstPersonCamera playerCamera;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private InteractiveHandler interactiveHandler;
    [SerializeField] private ItemGrab itemGrab;

    [SerializeField] private Transform cartPoint;

    public bool IsCameraPositionChanged;
    public Transform CartPoint => cartPoint;
    public FirstPersonCamera FirstPersonCamera => playerCamera;

    private HashSet<Type> interactivityBlockers = new();
    private HashSet<Type> movementBlockers = new();

    private void Awake()
    {
        Core.Camera.StateChanged += OnStateChanged;
    }

    private void OnStateChanged(CameraType type)
    {
        bool blockEnabled = type != CameraType.GameplayCamera;

        BlockControl(blockEnabled, this);
        BlockInteractivity(blockEnabled, this);
    }

    public void SetCartActivity(bool value)
    {
        playerController.IsHasCart = value;
        itemGrab.IsEnabled = !value;
    }

    public void BlockInteractivity(bool value, object blocker)
    {
        if(BlockHandle(interactivityBlockers, blocker, value))
        {
            interactiveHandler.ChangeEnabled(!value);
        }
    }

    public void BlockControl(bool value)
    {
        BlockControl(value, playerCamera);
    }

    public void BlockControl(bool value, object blocker)
    {
        if(BlockHandle(movementBlockers, blocker, value))
        {
            playerController.MovementBlockEnabled(value);
            FirstPersonCamera.SetCameraBlockEnabled(value);

            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    private bool BlockHandle(HashSet<Type> set, object blocker, bool value)
    {
        if(value == false)
        {
            if (set.Contains(blocker.GetType()))
            {
                set.Remove(blocker.GetType());
            }

            return set.Count == 0;
        }

        set.Add(blocker.GetType());

        return true;
    }
}
