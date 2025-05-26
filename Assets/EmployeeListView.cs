using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeListView : MonoBehaviour
{
    [SerializeField] private EmployeeView[] employeeViews;
    [SerializeField] private RatingBlock ratingBlock;

    [SerializeField] private EmployeeInfo employeeInfo;

    private int requiredRating = 10000;

    public void UpdateView(StoreConfig config, ShopInteractor interactor)
    {
        for (int i = 0; i < employeeViews.Length; i++)
        {
            if(i >= config.Employees.Length)
            {
                employeeViews[i].Hide();
                continue;
            }

            var employee = config.Employees[i];
            bool isHired = interactor.IsHired(config.Id, employee);
            int salary = interactor.GetSalary(employee);

            employeeViews[i].SetData(employee, salary, isHired);
            employeeViews[i].AssignListener(() => employeeInfo.ShowInfo(employee));
        }

        bool blockEnabled = Core.Interactors.GetInteractor<DayProgressInteractor>().GetRating() < requiredRating;
        ratingBlock.Block(requiredRating, blockEnabled);
    }

    public void Hide()
    {
        for (int i = 0; i < employeeViews.Length; i++)
        {
            employeeViews[i].Hide();
        }
    }
}
