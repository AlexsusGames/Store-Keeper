using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticDataProvider : DataProvider
{
    private const string KEY = "STATISCTIC_DATA";
    public StatisticData Data { get; private set; }

    public override void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            Data = JsonUtility.FromJson<StatisticData>(save);
        }
        else Data = new StatisticData();
    }

    public override void Save()
    {
        string save = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(KEY, save);
    }
}
[System.Serializable]
public class StatisticData
{
    public List<float> SuppliersRating;

    public string CompanyName;

    public int PerfectSupplies;
    public int TotalSupplies;
    public int BoxesSold;
    public int ProductsSpoiled;
    public int DaysPassed;

    public float TotalLost;
    public float TotalEarned;
}
