using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankDataProvider : DataProvider
{
    private const string KEY = "BANK_SAVE";

    public int money {  get; set; }
    public override void Load()
    {
        money = PlayerPrefs.GetInt(KEY, 0);
    }

    public override void Save()
    {
        PlayerPrefs.SetInt(KEY, money);
    }
}
