using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class StoreEditor : MonoBehaviour
{
    [SerializeField] private FurnitureSelectionEditor furnitureSelector;
    [SerializeField] private FurniturePositionEditor furniturePositionEditor;
    [SerializeField] private FurnitureInventoryView inventoryView;
    [SerializeField] private FurnitureHandlerMenu handleMenu;
    [SerializeField] private Transform floorSurface;

    [SerializeField] private StoreFactory factory;
    [SerializeField] private ProductFinder productFinder;
    [SerializeField] private Surface handSurface;

    private FurnitureDataProvider dataProvider;
    private StoreFurnitureConfigFinder configFinder;

    private FurniturePositionDataProvider positionDataProvider;
    private ProductsManager productsManager;
    private bool isBeingPlaced;

    private void Awake()
    {
        productsManager = new ProductsManager(productFinder);
        dataProvider = new();
        configFinder = new();
        positionDataProvider = new();
        factory.Init(configFinder);
    }

    private void Start()
    {
        Init();

        UpdateInventoryView();

        dataProvider.OnDataChange += UpdateInventoryView;

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
            dataProvider.RemoveFurniture(name);

            string id = positionDataProvider.GetFreeID();
            var obj = factory.Create(name);

            obj.FurnitureId = id;

            furnitureSelector.Select(obj);
            furniturePositionEditor.SetEditStatus(true);
        }
    }

    private void OnFurnitureSelected(FurniturePlacementView furnitureView)
    {
        var sprite = configFinder.FindByName(furnitureView.FurnitureName).shopSprite;

        UnityAction storeAction = () =>
        {
            if(!furnitureView.HasProducts())
            {
                dataProvider.AddFurniture(furnitureView.FurnitureName);
                furniturePositionEditor.SetEditStatus(false);
                furnitureSelector.Deselect();

                positionDataProvider.RemoveById(furnitureView.FurnitureId);

                Destroy(furnitureView.gameObject);
                isBeingPlaced = false;
            }
        };

        UnityAction confirmAction = () =>
        {
            furniturePositionEditor.Confirm();
            furnitureSelector.Deselect();

            positionDataProvider.AddFurniture(furniturePositionEditor.GetNewPosition());
            isBeingPlaced = false;
        };

        UnityAction editAction = () =>
        {
            if(!furnitureView.HasProducts())
            {
                furniturePositionEditor.SetEditStatus(true);
            }
        };

        furniturePositionEditor.DeselectAction = furnitureSelector.IsCreated ? new System.Action(storeAction) : null;

        if(furnitureSelector.IsCreated)
        {
            storeAction = () => { };
        }

        handleMenu.SetFurniture(sprite, storeAction, confirmAction, editAction);
    }

    private void OnDestroy()
    {
        dataProvider.OnDataChange -= UpdateInventoryView;
        productsManager.SavePosition(GetActiveSurfaces());
    }

    private void UpdateInventoryView()
    {
        var data = dataProvider.GetFurnitureList();
        UnityAction<string> action = CreateNewFurniture;
        inventoryView.SetData(data, action);
    }

    private void Init()
    {
        var positions = positionDataProvider.GetPositions();

        productsManager.PlaceProducts(string.Empty, floorSurface);

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

        return result;
    }
}
