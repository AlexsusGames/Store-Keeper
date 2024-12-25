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

    [SerializeField] private FurnitureFactory factory;

    private FurnitureDataProvider dataProvider;
    private StoreFurnitureConfigFinder configFinder;

    private FurniturePositionDataProvider positionDataProvider;
    private bool isBeingPlaced;

    private void Awake()
    {
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
            dataProvider.AddFurniture(furnitureView.FurnitureName);
            furniturePositionEditor.SetEditStatus(false);
            furnitureSelector.Deselect();

            positionDataProvider.RemoveById(furnitureView.FurnitureId);

            Destroy(furnitureView.gameObject);
            isBeingPlaced = false;
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
            furniturePositionEditor.SetEditStatus(true);
        };

        furniturePositionEditor.DeselectAction = furnitureSelector.IsCreated ? new System.Action(storeAction) : null;

        if(furnitureSelector.IsCreated)
        {
            storeAction = () => { };
        }

        handleMenu.SetFurniture(sprite, storeAction, confirmAction, editAction);
    }

    private void OnDestroy() => dataProvider.OnDataChange -= UpdateInventoryView;

    private void UpdateInventoryView()
    {
        var data = dataProvider.GetFurnitureList();
        UnityAction<string> action = CreateNewFurniture;
        inventoryView.SetData(data, action);
    }

    private void Init()
    {
        var positions = positionDataProvider.GetPositions();

        foreach(var position in positions)
        {
            Debug.Log(position.Name);

            var prefab = factory.Create(position.Name);

            prefab.FurnitureId = position.Id;

            prefab.transform.localPosition = position.Position;
            prefab.transform.localRotation = Quaternion.Euler(position.Rotation);
        }
    }
}
