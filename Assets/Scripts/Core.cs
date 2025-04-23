using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Core
{
    private static SoundManager soundManager;

    public static InteractorsBase Interactors;
    public static DataProviderBase DataProviders;

    public static CameraType LastCamera;
    public static CameraType StartCamera = CameraType.MainMenuCamera;

    public static bool IsInitialized;
    public static void Init()
    {
        DataProviders = new DataProviderBase();
        Interactors = new InteractorsBase();

        ProductList = new ProductList();
        Localization = new Localization();

        DataProviders.CreateAllDataProviders();
        Interactors.CreateAllInteractors();

        DataProviders.SendOnCreateAllDataProviders();
        Interactors.SendOnCreateAllInteractors();

        DataProviders.LoadAllDataProviders();
        Interactors.InitializeAllInteractors();

        DataProviders.SendOnStartAllDataProviders();
        Interactors.SendOnStartAllInteractors();

        Statistic = Interactors.GetInteractor<StatisticInteractor>();

        IsInitialized = true;
    }
    public static void SaveData() => DataProviders.SaveAllDataProviders();

    public static SoundManager Sound
    {
        get
        {
            if (soundManager == null)
            {
                soundManager = GameObject.FindObjectOfType<SoundManager>();
            }

            return soundManager;
        }
        set { soundManager = value; }
    }

    private static CluesManager cluesManager;

    public static CluesManager Clues
    {
        get
        {
            if (cluesManager == null)
            {
                cluesManager = GameObject.FindObjectOfType<CluesManager>();
            }

            return cluesManager;
        }

        set { cluesManager = value; }
    }

    private static CameraStateChanger cameraStateChanger;

    public static CameraStateChanger Camera
    {
        get
        {
            if (cameraStateChanger == null)
                cameraStateChanger = GameObject.FindObjectOfType<CameraStateChanger>();

            return cameraStateChanger;
        }
        set { cameraStateChanger = value; }
    }

    private static QuestCreator questCreator;

    public static QuestCreator Quest
    {
        get
        {
            if(questCreator == null)
                questCreator = GameObject.FindObjectOfType<QuestCreator>();

            return questCreator;
        }
        set { questCreator = value; }
    }

    public static StatisticInteractor Statistic;
    public static ProductList ProductList;
    public static Localization Localization;
}
