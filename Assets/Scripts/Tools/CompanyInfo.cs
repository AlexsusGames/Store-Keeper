using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyInfo 
{
    private string[] companyNames = { "FreshMart LLC", "The Veg Bar Inc.", "MooMart LLC", "CityHall", "Bread Nook", "Tech Town" };
    private string[] companyMessages = { "Could you deliver some items, please?", "Can I get a delivery of goods?", "Would you mind bringing over some supplies?",
    "I need a delivery � can you help?", "Any chance you could drop off my order?", "Can you bring me some stuff?", "Could I have some goods delivered?",
    "Would you be able to deliver a package?", "Can I place a delivery order?", "I�m looking to get something delivered."};

    private Dictionary<EmployeeType, string> employeeInfo = new Dictionary<EmployeeType, string>()
    {
        {EmployeeType.Manager, "manager_info" },
        {EmployeeType.Accountant, "accountant_info" },
        {EmployeeType.Salesperson, "salesperson_info" },
        {EmployeeType.SecurityGuard, "security_info" },
        {EmployeeType.Loader, "loader_info" },
        {EmployeeType.Cashier, "cashier_info" },
        {EmployeeType.DriverHelper, "driver_info" },
    };

    public string GetEmployeeInfo(EmployeeType employeeType) => employeeInfo[employeeType];

    public string GetCompanyName(CompanyType type) => companyNames[(int)type];

    public string GetRandomCompanyMessage()
    {
        int random = Random.Range(0, companyMessages.Length);
        return companyMessages[random];
    }
}
