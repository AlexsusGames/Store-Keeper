using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayProgressDataProvider : DataProvider
{
    private const string KEY = "DAY_PROGRESS";

    public DailyRoutineData Data { get; private set; }
    public override void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            Data = JsonUtility.FromJson<DailyRoutineData>(save);
        }
        else { Data = new DailyRoutineData(); }
    }

    public override void Save()
    {
        string save = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(KEY, save);
    }
}
[System.Serializable]
public class DailyRoutineData
{
    public List<CarType> CompletedCars;
    public float CurrentLosses;
}
