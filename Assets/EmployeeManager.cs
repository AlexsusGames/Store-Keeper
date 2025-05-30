using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeManager : MonoBehaviour
{
    [SerializeField] private GameObject[] employeesObjects;
    [SerializeField] private EmployeeType[] employees;

    private void Start()
    {
        var interactor = Core.Interactors.GetInteractor<ShopInteractor>();
        string storeId = "store";
        
        for (int i = 0; i < employees.Length; i++)
        {
            var employee = employees[i];

            bool isHired = interactor.IsHired(storeId, employee);

            employeesObjects[i].SetActive(isHired);
        }
    }

    public bool IsHired(EmployeeType employeeType)
    {
        for (int i = 0; i < employees.Length; i++)
        {
            if (employees[i] == employeeType)
            {
                return employeesObjects[i].activeInHierarchy;
            }
        }

        return false;
    }
}
