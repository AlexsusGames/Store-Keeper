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
    [SerializeField] private StoreFactory factory;

    private StoragePlacementInteractor storagePlacement;

    private bool isBeingPlaced;

    private void Awake()
    {
        storagePlacement = Core.Interactors.GetInteractor<StoragePlacementInteractor>();

        Subscribe();
    }

    public void CreateNewFurniture(string name)
    {
        Debug.Log(name);

        if(!isBeingPlaced && !furnitureSelector.IsSelected)
        {
            isBeingPlaced = true;

            string id = storagePlacement.GetFreeID();
            var obj = factory.Create(name);

            obj.FurnitureId = id;

            furnitureSelector.Select(obj);
            furniturePositionEditor.SetEditStatus(true);
        }

        else Core.Clues.Show("First, confirm the storage container's placement");
    }

    private void OnFurnitureSelected(FurniturePlacementView furnitureView)
    {
        UnityAction storeAction = () =>
        {
            if(!furnitureView.HasProducts())
            {
                if(!furnitureSelector.IsCreated)
                {
                    storagePlacement.Remove(furnitureView.FurnitureName, furnitureView.FurnitureId);
                }

                furniturePositionEditor.SetEditStatus(false);
                furnitureSelector.Deselect();

                Destroy(furnitureView.gameObject);
                isBeingPlaced = false;

                Core.Sound.PlayClip(AudioType.StorageFold);
            }

            else Core.Clues.Show("Before editing the storage unit's placement, remove all products from it.");
        };

        UnityAction confirmAction = () =>
        {
            furniturePositionEditor.Confirm();
            furnitureSelector.Deselect();

            var data = furniturePositionEditor.GetNewPosition();
            storagePlacement.Place(data, furnitureView.FurnitureName);

            isBeingPlaced = false;

            Core.Quest.TryChangeQuest(QuestType.PlaceStorage);
            Core.Sound.PlayClip(AudioType.StorageFold);
        };

        UnityAction editAction = () =>
        {
            if(!furnitureView.HasProducts())
            {
                furniturePositionEditor.SetEditStatus(true);
            }

            else Core.Clues.Show("Before editing the storage unit's placement, remove all products from it.");
        };

        furniturePositionEditor.DeselectAction = furnitureSelector.IsCreated ? new System.Action(storeAction) : null;

        handleMenu.SetFurniture(furnitureView.FurnitureName, storeAction, confirmAction, editAction);
    }

    public void UpdateInventoryView()
    {
        var data = storagePlacement.NonPlacedStorages;
        UnityAction<string> action = CreateNewFurniture;
        inventoryView.SetData(data, action);
    }

    public void Init()
    {
        UpdateInventoryView();

        var positions = storagePlacement.PlacedStorages;

        foreach(var position in positions)
        {
            var prefab = factory.Create(position.Name);

            prefab.FurnitureId = position.Id;

            prefab.transform.localPosition = position.Position;
            prefab.transform.localRotation = Quaternion.Euler(position.Rotation);
        }
    }

    private void Subscribe()
    {
        storagePlacement.OnChanged += UpdateInventoryView;
        furnitureSelector.OnSelected += OnFurnitureSelected;
        furnitureSelector.OnSelected += furniturePositionEditor.OnFurnitureSelected;
        furnitureSelector.OnSelectionCanceled += handleMenu.Hide;
        furnitureSelector.OnSelectionCanceled += furniturePositionEditor.OnDeselect;
        furniturePositionEditor.IsAvailablePosition += handleMenu.ChangeConfirmButtonInterractable;
        furnitureSelector.OnConfirmed += () => furniturePositionEditor.SetEditStatus(false);
    }

    public void Save()
    {  
    }

    public void Load()
    {
        Init();
    }
}
