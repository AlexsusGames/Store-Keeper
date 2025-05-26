using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsManager : MonoBehaviour
{
    [SerializeField] private ShopStoreButton[] buttons;

    private StoreConfig[] stores;
    private ShopInteractor interactor;

    private void OnEnable()
    {
        UpdateView();
    }

    private void Init()
    {
        if (interactor == null) 
            interactor = Core.Interactors.GetInteractor<ShopInteractor>();

        if(stores == null)
            stores = Resources.LoadAll<StoreConfig>("Stores");
    }

    private void UpdateView()
    {
        Init();

        HideAll();

        for(int i = 0; i < stores.Length; i++)
        {
            string id = stores[i].Id;
            var button = GetFreeButton();

            if (interactor.IsRenting(id))
            {
                button.Init(interactor, stores[i]);
            }
        }
    }

    private void HideAll()
    {
        for(int i = 0;i < buttons.Length; i++)
        {
            buttons[i].Hide();
        }
    }

    private ShopStoreButton GetFreeButton()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if (!buttons[i].IsInitialized) return buttons[i];
        }

        throw new System.Exception("There isn't enough views");
    }
}
