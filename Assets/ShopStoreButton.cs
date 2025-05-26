using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopStoreButton : MonoBehaviour
{
    [SerializeField] private ProductListView view;

    private Button button;

    public bool IsInitialized {  get; private set; }

    public void Init(ShopInteractor interactor, StoreConfig config)
    {
        button = GetComponent<Button>();

        gameObject.SetActive(true);

        button.image.sprite = config.Icon;

        UnityAction action = () =>
        {
            Core.Sound.PlayClip(AudioType.MouseClick);

            if (config.Id != "store")
            {
                var products = interactor.GetProductMap(config.Id);

                view.ShowProductList(products, true);
            }
            else view.ShowStoreProducts();
        };

        AssignListener(action);

        IsInitialized = true;
    }

    public void Hide()
    {
        IsInitialized = false;
        gameObject.SetActive(false);
    }

    private void AssignListener(UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
}
