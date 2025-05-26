using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopRentInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text companyName;
    [SerializeField] private TMP_Text rentText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private Image iconImage;

    [SerializeField] private TMP_Text unpaidRentText;
    [SerializeField] private TMP_Text salaryText;

    [SerializeField] private GameObject rentTools;

    public void SetData(StoreConfig storeConfig, ShopInteractor interactor)
    {
        gameObject.SetActive(true);

        bool isRenting = interactor.IsRenting(storeConfig.Id);

        unpaidRentText.text = $"${interactor.GetUnpaidRent(storeConfig.Id)}";
        salaryText.text = $"${interactor.GetSalary(storeConfig.Id)}";

        UpdateView(storeConfig, isRenting);
    }

    private void UpdateView(StoreConfig config, bool isRenting)
    {
        iconImage.sprite = config.Icon;

        string translatedType = Core.Localization.Translate(config.CompanyType.ToString());
        string translatedRent = Core.Localization.Translate("Rental price:");
        string translatedRenter = Core.Localization.Translate("Renter:");

        companyName.text = isRenting ? $"{translatedRenter} <color=green>{Core.Statistic.GetCompanyName()}" : "";

        rentTools.SetActive(isRenting && config.RentCost != 0);

        rentText.text = $"{translatedRent} <color=green>${config.RentCost}";
        typeText.text = translatedType;
    }

    public void Hide() => gameObject.SetActive(false);
}
