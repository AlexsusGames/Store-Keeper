using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader<T> 
{
    private T data;

    public T Load(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string save = PlayerPrefs.GetString(key);
            data = JsonUtility.FromJson<T>(save);
        }

        return data;
    }

    public void Save(T data, string key)
    {
        string save = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, save);
    }
}
