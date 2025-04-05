using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankDataProvider : DataProvider
{
    private const string KEY = "BANK_SAVE";
    private const string DAY_PROFIT_KEY = "DAY_PROFIT";

    private const float STANDART_AMOUNT = 5000f;

    public float Money {  get; set; }
    public float DayProfit { get; set; }
    public override void Load()
    {
        Money = PlayerPrefs.GetFloat(KEY, STANDART_AMOUNT);
        DayProfit = PlayerPrefs.GetFloat(DAY_PROFIT_KEY, 0);
    }

    public override void Save()
    {
        PlayerPrefs.SetFloat(KEY, Money);
        PlayerPrefs.SetFloat(DAY_PROFIT_KEY, DayProfit);
    }
}
