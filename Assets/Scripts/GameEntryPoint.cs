using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] dataProviders;
    [SerializeField] private ProductSupplyManager productSupplyManager;
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private SupplyConfig test;
    [SerializeField] private DialogConfig testDialog;
    [SerializeField] private Tutorial tutor;

    [Inject] private SettingsDataProvider settingsDataProvider;

    private void Awake()
    {
        Core.Init();

        settingsDataProvider.OnSettingsChanged += ApplySettings;
    }

    private void Start()
    {
        Init();

        Core.Camera.Init();

        settingsDataProvider.ApplySettings();

        if(!tutor.IsCompleted)
        {
            if (Core.Camera.IsActive(CameraType.MainMenuCamera))
            {
                Core.Camera.StateChanged += StartDialog;
            }
            else tutor.StartTutor();
        }
    }

    public void Init()
    {
        productSupplyManager.SetData(test);

        for (int i = 0; i < dataProviders.Length; i++)
        {
            if (dataProviders[i].TryGetComponent(out IDataProvider data))
            {
                data.Load();
            }
        }
    }

    public void SaveData()
    {
        for (int i = 0; i < dataProviders.Length; i++)
        {
            if (dataProviders[i].TryGetComponent(out IDataProvider data))
            {
                data.Save();
            }
        }

        Core.SaveData();
    }

    private void ApplySettings(Settings settings)
    {
        Screen.fullScreen = settings.IsFullScreen;

        QualitySettings.SetQualityLevel(settings.QualityIndex);

        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
    }

    private void StartDialog(CameraType type)
    {
        if(type == CameraType.GameplayCamera)
        {
            tutor.StartTutor();

            Core.Camera.StateChanged -= StartDialog;
        }
    }
}
