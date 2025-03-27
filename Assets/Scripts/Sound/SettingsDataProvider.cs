using System;
using System.Collections.Generic;
using UnityEngine;

public class SettingsDataProvider
{
    private Settings data;
    private const string KEY = "SoundDataSave";

    public event Action<Settings> OnSettingsChanged;

    private void SaveData()
    {
        string save = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(KEY, save);
    }

    public void ApplySettings() => OnSettingsChanged?.Invoke(data);

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            data = JsonUtility.FromJson<Settings>(save);
        }
        else
        {
            data = new Settings()
            {
                Music = 0.5f,
                Sound = 0.5f,
                MouseSensivity = 500,
                IsFullScreen = true,
                QualityIndex = 2,
                ResolutionIndex = -1,
            };
        }
    }

    public Settings GetData()
    {
        LoadData();
        return data;
    }

    public void ChangeSound(float newValue)
    {
        data.Sound = newValue;
        ApplySettings();

        SaveData();
    }

    public void ChangeQuality(int index)
    {
        data.QualityIndex = index;
        ApplySettings();

        SaveData();
    }

    public void ChangeMusic(float newValue)
    {
        data.Music = newValue;
        ApplySettings();

        SaveData();
    }

    public void ChangeSensivity(float value)
    {
        data.MouseSensivity = value;
        ApplySettings();

        SaveData();
    }

    public void ChangeResolution(int index)
    {
        data.ResolutionIndex = index;
        ApplySettings();

        SaveData();
    }

    public void ChangeFullScreen(bool value)
    {
        data.IsFullScreen = value;
        ApplySettings();

        SaveData();
    }

    public void SetData(Settings data)
    {
        this.data = data;
        ApplySettings();

        SaveData();
    }
}

[System.Serializable]
public class Settings
{
    public float Music;
    public float Sound;
    public float MouseSensivity;
    public int ResolutionIndex;
    public int QualityIndex;
    public bool IsFullScreen;
}
