using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/StoreConfig", fileName = "StoreConfig")]
public class StoreConfig : ScriptableObject
{
    public string Id;
    public Sprite Icon;

    public CompanyType CompanyType;
    public EmployeeType[] Employees;

    public int RentCost;
    public int RequiredRating;
}
public enum EmployeeType
{
    Cashier,
    Salesperson,
    Manager,
    Loader,
    Accountant,
    DriverHelper,
    SecurityGuard
}
