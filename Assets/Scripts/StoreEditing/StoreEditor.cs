using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Zenject;

public class StoreEditor : MonoBehaviour, IDataProvider
{
    [SerializeField] private FurnitureSelectionEditor furnitureSelector;
    [SerializeField] private FurniturePositionEditor furniturePositionEditor;
    [SerializeField] private FurnitureInventoryView inventoryView;
    [SerializeField] private FurnitureHandlerMenu handleMenu;
    [SerializeField] private Transform floorSurface;

    [SerializeField] private StoreFactory factory;
    [SerializeField] private Surface handSurface;
    [SerializeField] private Surface cartSurface;

    [Inject] private ProductFinder productFinder;

    private NonPlacedFurnitureDataProvider nonPlacedFurnitureData;
    private PlacedFurnitureDataProvider placedFurnitureData;

    private StoreFurnitureConfigFinder configFinder;

    private ProductsManager productsManager;
    private bool isBeingPlaced;
    public NonPlacedFurnitureDataProvider NonPlacedFurnitureData => nonPlacedFurnitureData;

    private void Awake()
    {
        productsManager = new ProductsManager(productFinder);

        nonPlacedFurnitureData = new();
        configFinder = new();
        placedFurnitureData = new();

        factory.Init(configFinder);

        nonPlacedFurnitureData.OnDataChange += UpdateInventoryView;

        furnitureSelector.OnSelected += OnFurnitureSelected;
        furnitureSelector.OnSelected += furniturePositionEditor.OnFurnitureSelected;

        furnitureSelector.OnSelectionCanceled += handleMenu.Hide;
        furnitureSelector.OnSelectionCanceled += furniturePositionEditor.OnDeselect;

        furniturePositionEditor.IsAvailablePosition += handleMenu.ChangeConfirmButtonInterractable;

        furnitureSelector.OnConfirmed += () => furniturePositionEditor.SetEditStatus(false);
    }

    public void CreateNewFurniture(string name)
    {
        if(!isBeingPlaced && !furnitureSelector.IsSelected)
        {
            isBeingPlaced = true;
            nonPlacedFurnitureData.RemoveFurniture(name);

            string id = placedFurnitureData.GetFreeID();
            var obj = factory.Create(name);

            obj.FurnitureId = id;

            furnitureSelector.Select(obj);
            furniturePositionEditor.SetEditStatus(true);
        }

        else CluesManager.instance.ShowClue("First, confirm the storage container's placement");
    }

    private void OnFurnitureSelected(FurniturePlacementView furnitureView)
    {
        var sprite = configFinder.FindByName(furnitureView.FurnitureName).shopSprite;

        UnityAction storeAction = () =>
        {
            if(!furnitureView.HasProducts())
            {
                nonPlacedFurnitureData.AddFurniture(furnitureView.FurnitureName);
                furniturePositionEditor.SetEditStatus(false);
                furnitureSelector.Deselect();

                placedFurnitureData.RemoveById(furnitureView.FurnitureId);

                Destroy(furnitureView.gameObject);
                isBeingPlaced = false;
            }

            else CluesManager.instance.ShowClue("Before editing the storage unit's placement, remove all products from it.");
        };

        UnityAction confirmAction = () =>
        {
            furniturePositionEditor.Confirm();
            furnitureSelector.Deselect();

            placedFurnitureData.AddFurniture(furniturePositionEditor.GetNewPosition());
            isBeingPlaced = false;
        };

        UnityAction editAction = () =>
        {
            if(!furnitureView.HasProducts())
            {
                furniturePositionEditor.SetEditStatus(true);
            }

            else CluesManager.instance.ShowClue("Before editing the storage unit's placement, remove all products from it.");
        };

        furniturePositionEditor.DeselectAction = furnitureSelector.IsCreated ? new System.Action(storeAction) : null;

        if(furnitureSelector.IsCreated)
        {
            storeAction = () => { };
        }

        handleMenu.SetFurniture(sprite, storeAction, confirmAction, editAction);
    }

    private void UpdateInventoryView()
    {
        var data = nonPlacedFurnitureData.GetFurnitureList();
        UnityAction<string> action = CreateNewFurniture;
        inventoryView.SetData(data, action);
    }

    private void Init()
    {
        var positions = placedFurnitureData.GetPositions();

        productsManager.PlaceProducts(string.Empty, floorSurface);
        productsManager.PlaceProducts(cartSurface.GetSurfaceId(), cartSurface.transform);

        foreach(var position in positions)
        {
            var prefab = factory.Create(position.Name);

            prefab.FurnitureId = position.Id;

            prefab.transform.localPosition = position.Position;
            prefab.transform.localRotation = Quaternion.Euler(position.Rotation);

            productsManager.PlaceProducts(prefab.FurnitureId, prefab.GetFirstSurface());
        }
    }

    public PickupObject GetGrabbedSavedObject()
    {
        productsManager.PlaceProducts(handSurface.GetSurfaceId(), handSurface.transform);
        
        if(handSurface.transform.childCount > 0)
        {
            handSurface.transform.GetChild(0).TryGetComponent(out PickupObject obj);
            return obj;
        }

        return null;
    }

    public List<Surface> GetActiveSurfaces()
    {
        var childCount = transform.childCount;
        List<Surface> result = new List<Surface>();

        for (int i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(i);

            if (child.TryGetComponent(out Surface surface))
            {
                result.Add(surface);
            }
        }

        result.Add(handSurface);
        result.Add(cartSurface);

        return result;
    }

    public void Save()
    {
        productsManager.SavePosition(GetActiveSurfaces());
        placedFurnitureData.SaveData();
        nonPlacedFurnitureData.SaveData();
    }

    public void Load()
    {
        Init();

        UpdateInventoryView();
    }
}
