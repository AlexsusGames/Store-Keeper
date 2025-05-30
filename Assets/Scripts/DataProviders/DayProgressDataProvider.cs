using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayProgressDataProvider : DataProvider
{
    private const string KEY = "DAY_PROGRESS";

    public SupplyData Data { get; private set; }
    public override void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            Data = JsonUtility.FromJson<SupplyData>(save);
        }
        else Data = new SupplyData();
    }

    public override void Save()
    {
        string save = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(KEY, save);
    }
}
[System.Serializable]
public class SupplyData
{
    public List<string> DayCarsID;
    public int Day;

    public int Rating;

    public List<string> CarsPassed;
    public List<float> TaxToPay;
}
