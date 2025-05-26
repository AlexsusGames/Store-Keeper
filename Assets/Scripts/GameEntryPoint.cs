using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] dataProviders;
    [SerializeField] private ProductSupplyManager productSupplyManager;
    [SerializeField] private SupplyConditions supplyConditions;
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private Tutorial tutor;
    [SerializeField] private TruckAmountView truckAmountView;

    [Inject] private SettingsDataProvider settingsDataProvider;
    [Inject] private SupplyCreator supplyCreator;

    private void Awake()
    {
        Core.Init();

        settingsDataProvider.OnSettingsChanged += ApplySettings;
    }

    private void Start()
    {
        Init();

        settingsDataProvider.ApplySettings();

        if (Core.Camera.IsActive(CameraType.MainMenuCamera))
        {
            Core.Camera.StateChanged += StartDialog;
        }
        else tutor.StartQuestLine();
    }

    public void Init()
    {
        supplyCreator.Init();
        truckAmountView.Init();

        var carList = supplyConditions.GetCarsByCompletedConditions();
        var supplyList = supplyCreator.CreateSupplyList(carList);

        productSupplyManager.SetData(supplyList);

        for (int i = 0; i < dataProviders.Length; i++)
        {
            if (dataProviders[i].TryGetComponent(out IDataProvider data))
            {
                data.Load();
            }
        }

        Core.Camera.Init();
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
            tutor.StartQuestLine();

            Core.Camera.StateChanged -= StartDialog;
        }
    }
}
