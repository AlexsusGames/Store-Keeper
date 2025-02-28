using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] dataProviders;
    [SerializeField] private ProductSupplyManager productSupplyManager;
    [SerializeField] private SupplyConfig test;

    private void Awake() => Time.timeScale = 1;

    private void Start()
    {
        Init();

        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
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
    }

    private void OnDisable()
    {
        //SaveData();
    }
}
