using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSupplyManager : MonoBehaviour
{
    [SerializeField] private DeliveringManager deliveringManager;
    [SerializeField] private ShopManager shopManager;

    [SerializeField] private GameEntryPoint gameEntryPoint;

    private ShopInteractor shopInteractor;
    private DeliveryInteractor deliveryInteractor;
    private StoreConfig storeConfig;

    public void Init(ShopInteractor interactor, StoreConfig storeConfig)
    {
        this.shopInteractor = interactor;
        this.storeConfig = storeConfig;

        deliveryInteractor = Core.Interactors.GetInteractor<DeliveryInteractor>();
    }

    public bool TryStartSupply()
    {
        Core.Sound.PlayClip(AudioType.MouseClick);

        if (deliveryInteractor.GetRemainingTrucks() == 0)
        {
            Core.Clues.Show("All trucks are currently in use. A truck will be available the next day after delivery.");
            return false;
        }

        if (deliveringManager.TryDeliverProducts(null))
        {
            gameObject.SetActive(true);
            deliveryInteractor.OnStartDelivery(null);
            return true;
        }
        return false;
    }

    public void Cancel()
    {
        Core.Sound.PlayClip(AudioType.MouseClick);

        if (deliveringManager.CancelDelivering())
        {
            gameObject.SetActive(false);
            shopManager.CancelSupply();

            deliveryInteractor.CancelDelivery(null);
        }
    }

    public void FinishDelivering()
    {
        Core.Sound.PlayClip(AudioType.MouseClick);

        if (deliveringManager.DeliverToShop(shopInteractor, storeConfig))
        {
            gameObject.SetActive(false);
            shopManager.CancelSupply();

            gameEntryPoint.SaveData();
        }
    }
}
