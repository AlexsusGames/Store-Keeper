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

    [Inject] private ProductFinder productFinder;

    private void OnEnable()
    {
        var products = Core.ProductList.GetProductMap();

        foreach (var product in products.Keys)
        {
            var config = productFinder.FindByName(product);
            var view = pool.Get(parent);

            string name = Core.Localization.Translate(product);
            string amount = $"{products[product]} {Core.Localization.Translate(config.MeasureType.ToString())}";

            view.SetData(name, amount);

            UnityAction action = () =>
            {
                priceEditor.ShowInfo(product);
                Core.Sound.PlayClip(AudioType.MouseClick);
            };

            view.AssignAction(action);
        }
    }

    private void OnDisable()
    {
        pool.HideAll();
        priceEditor.Hide();
    }
}
