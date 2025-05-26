using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class ProductListView : MonoBehaviour
{
    [SerializeField] private ProductViewPool pool;
    [SerializeField] private RectTransform parent;
    [SerializeField] private PriceEditor priceEditor;

    [SerializeField] private ProductSupplyManager supplyManager;

    [Inject] private ProductFinder productFinder;

    private bool showAmount => !supplyManager.IsBeingSupplied;

    private void Start()
    {
        ShowStoreProducts();
    }

    public void ShowStoreProducts()
    {
        var products = Core.ProductList.GetProductMap();

        ShowProductList(products, false);
    }

    public void ShowProductList(Dictionary<string, float> products, bool showAmount = true)
    {
        HideOldList();

        foreach (var product in products.Keys)
        {
            if (products[product] == 0)
                continue;

            var config = productFinder.FindByName(product);
            var view = pool.Get(parent);

            string name = Core.Localization.Translate(product);
            string amount = $"{products[product]} {Core.Localization.Translate(config.MeasureType.ToString())}";

            bool allowShowingProducts = showAmount || this.showAmount;

            view.SetData(name, amount, allowShowingProducts);

            UnityAction action = () =>
            {
                priceEditor.ShowInfo(product);
                Core.Sound.PlayClip(AudioType.MouseClick);
            };

            view.AssignAction(action);
        }
    }

    public void HideOldList() => pool.HideAll();

    private void OnDisable()
    {
        pool.HideAll();
        priceEditor.Hide();
    }
}
