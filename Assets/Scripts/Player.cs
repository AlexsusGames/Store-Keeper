using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDataProvider
{
    [SerializeField] private FirstPersonCamera playerCamera;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private InteractiveHandler interactiveHandler;

    public FirstPersonCamera FirstPersonCamera => playerCamera;
    public FirstPersonController PlayerController => playerController;
    public InteractiveHandler InteractiveHandler => interactiveHandler;

    private const string KEY = "player_data_save";
    private PlayerData playerData;

    private void Awake()
    {
        playerController.TabletActivity = false;
    }

    public void BlockControl(bool value)
    {
        PlayerController.MovementBlockEnabled(value);
        FirstPersonCamera.SetCameraBlockEnabled(value);

        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            playerData = JsonUtility.FromJson<PlayerData>(save);

            playerController.PlayerPosition = playerData.Position;
        }
    }

    public void Save()
    {
        playerData = new PlayerData()
        {
            Position = playerController.PlayerPosition,
        };

        string save = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(KEY, save);
    }
}
