using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInfoButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private StoreConfig config;

    [SerializeField] private ShopManager shopInfo;

    [SerializeField] private Color boughtColor;
    [SerializeField] private Color standartColor;

    private ShopInteractor interactor;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        interactor = Core.Interactors.GetInteractor<ShopInteractor>();

        UpdateView();

        button.onClick.AddListener(() => shopInfo.SetData(config));

        interactor.OnRentChanged += UpdateView;
    }

    public void UpdateView()
    {
        Color color = interactor.IsRenting(config.Id) ? boughtColor : standartColor;
        button.image.color = color;
    }
}
