using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PriceEditor : MonoBehaviour
{
    [SerializeField] private CompanyTypeView[] companyTypes;

    [SerializeField] private TMP_Text deliveryPrice;
    [SerializeField] private TMP_Text shopPrice;
    [SerializeField] private TMP_Text marketPrice;
    [SerializeField] private TMP_Text boxWeight;

    [SerializeField] private TMP_Text nameText;

    [SerializeField] private Slider deliverySlider;
    [SerializeField] private Slider shopSlider;

    [SerializeField] private Toggle toggle;

    [Inject] private ProductFinder productFinder;

    private PricingInteractor interactor;
    private string cachedProduct;

    public void ShowInfo(string productName)
    {
        if(interactor == null)
            interactor = Core.Interactors.GetInteractor<PricingInteractor>();

        cachedProduct = productName;

        nameText.text = Core.Localization.Translate(cachedProduct);
        toggle.isOn = interactor.IsForSale(cachedProduct);

        float deliveryPrice = interactor.GetDeliveryPrice(productName);
        SetDeliveryPrice(deliveryPrice);
        
        float shopPrice = interactor.GetShopPrice(productName);
        SetShopPrice(shopPrice);

        float marketPrice = interactor.GetMarketPrice(productName);
        SetMarketPrice(marketPrice);

        float boxWeight = interactor.GetBoxWeigh(productName);
        SetBoxWeight(boxWeight);

        AssignSliders(marketPrice, shopPrice, deliveryPrice);

        UpdateCompanyTypes(productName);

        Enabled(true);
    }

    private void UpdateCompanyTypes(string productName)
    {
        var config = productFinder.FindByName(productName);
        var list = config.CompanyTypes.ToList();

        for (int i = 0; i < companyTypes.Length; i++)
        {
            companyTypes[i].UpdateView(list);
        }
    }

    private void ChangeSelling(bool value) => interactor.ChangeSelling(cachedProduct, value);

    private void AssignSliders(float marketPrice, float shopPrice, float deliveryPrice)
    {
        deliverySlider.onValueChanged.RemoveAllListeners();
        shopSlider.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.RemoveAllListeners();

        toggle.onValueChanged.AddListener(ChangeSelling);

        deliverySlider.minValue = marketPrice;
        shopSlider.minValue = marketPrice;

        deliverySlider.maxValue = marketPrice * 2;
        shopSlider.maxValue = marketPrice * 3;

        deliverySlider.value = deliveryPrice;
        shopSlider.value = shopPrice;

        deliverySlider.onValueChanged.AddListener(OnDeliveryPriceUpdate);
        shopSlider.onValueChanged.AddListener(OnShopPriceUpdate);
    }

    private void OnDeliveryPriceUpdate(float value)
    {
        float step = 0.25f;

        float snappedValue = Mathf.Round(value / step) * step;

        interactor.SetDeliveryPrice(cachedProduct, snappedValue);

        SetDeliveryPrice(snappedValue);
    }

    private void OnShopPriceUpdate(float value)
    {
        float step = 0.25f;

        float snappedValue = Mathf.Round(value / step) * step;

        interactor.SetShopPrice(cachedProduct, snappedValue);

        SetShopPrice(snappedValue);
    }

    private void Enabled(bool enabed)
    {
        gameObject.SetActive(enabed);
    }

    private void SetBoxWeight(float weight)
    {
        string translated = Core.Localization.Translate("Box weight:");
        string measureTranslated = Core.Localization.Translate("kg");

        boxWeight.text = $"{translated} <color=yellow>{weight}</color> {measureTranslated}";
    }

    private void SetDeliveryPrice(float price)
    {
        string translated = Core.Localization.Translate("Price on delivery:");

        deliveryPrice.text = $"{translated} $<color=yellow>{price}</color>";
    }

    private void SetShopPrice(float price)
    {
        string translated = Core.Localization.Translate("Retail price:");

        shopPrice.text = $"{translated} $<color=yellow>{price}</color>";
    }

    private void SetMarketPrice(float price)
    {
        string translated = Core.Localization.Translate("Supplier price:");

        marketPrice.text = $"{translated} $<color=yellow>{price}</color>";
    }

    public void Hide() => Enabled(false);
}
