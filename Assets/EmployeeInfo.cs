using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text employeeType;
    [SerializeField] private TMP_Text employeeDescribrion;

    [SerializeField] private Button hireButton;
    [SerializeField] private Button fireButton;

    public event Action OnEmploymentChanged;

    private StoreConfig config;
    private ShopInteractor interactor;

    private CompanyInfo info;

    private EmployeeType type;

    private void Awake()
    {
        hireButton.onClick.AddListener(Hire);
        fireButton.onClick.AddListener(Fire);

        info = new();
    }

    public void Init(StoreConfig config, ShopInteractor interactor)
    {
        this.config = config;
        this.interactor = interactor;
    }

    public void ShowInfo(EmployeeType type)
    {
        Core.Sound.PlayClip(AudioType.MouseClick);

        this.type = type;

        gameObject.SetActive(true);

        bool isHired = interactor.IsHired(config.Id, type);

        string translatedType = Core.Localization.Translate(type.ToString());
        string translatedDescribrion = Core.Localization.Translate(info.GetEmployeeInfo(type));

        employeeType.text = translatedType;
        employeeDescribrion.text = translatedDescribrion;

        UpdateActions(isHired);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);

        Core.Sound.PlayClip(AudioType.MouseClick);
    }

    private void UpdateActions(bool isHired)
    {
        hireButton.interactable = !isHired;
        fireButton.interactable = isHired;
    }

    private void Hire()
    {
        if (interactor.IsRenting(config.Id))
        {
            interactor.HireEmployee(config.Id, type);
            OnEmploymentChanged?.Invoke();

            Cancel();
        }
        else Core.Clues.Show("You need a rented space to hire employees.");
    }
    private void Fire()
    {
        interactor.FireEmployee(config.Id, type);
        OnEmploymentChanged?.Invoke();

        Cancel();
    }
}
