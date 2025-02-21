using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDataProvider
{
    [SerializeField] private FirstPersonCamera playerCamera;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private InteractiveHandler interactiveHandler;
    [SerializeField] private Transform cartPoint;
    public Transform CartPoint => cartPoint;
    public FirstPersonCamera FirstPersonCamera => playerCamera;
    public InteractiveHandler InteractiveHandler => interactiveHandler;

    private const string KEY = "player_data_save";
    private DataLoader<PositionData> dataLoader;

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

    public void Load()
    {
        dataLoader = new();

        var data = dataLoader.Load(KEY);

        if (data != null)
        {
            playerController.PlayerPosition = data.Position;
        }
    }

    public void Save()
    {
        PositionData data = new PositionData()
        {
            Position = playerController.PlayerPosition,
        };

        dataLoader.Save(data, KEY);
    }
}
