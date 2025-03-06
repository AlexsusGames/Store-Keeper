using System;
using UnityEngine;
using UnityEngine.Events;

public class FurniturePositionEditor : MonoBehaviour
{
    [SerializeField] private Camera topDownCamera;
    [SerializeField] private Transform parent; 

    private FurniturePlacementView cachedView;
    private GameObject editablePrefab => cachedView.gameObject;
    private Vector3 cachedPosition;
    private Quaternion cachedRotation;

    public event Action<bool> IsAvailablePosition;
    private bool isAvailable = true;

    private bool isEditing;
    private bool isConfirmed;

    public Action DeselectAction;

    public FurniturePositionData GetNewPosition()
    {
        return new FurniturePositionData()
        {
            Id = cachedView.FurnitureId,
            Name = cachedView.FurnitureName,
            Position = cachedView.transform.localPosition,
            Rotation = cachedView.transform.localRotation.eulerAngles
        };
    }
    public void OnFurnitureSelected(FurniturePlacementView furniture)
    {
        cachedView = furniture;
        cachedPosition = furniture.transform.localPosition;
        cachedRotation = furniture.transform.localRotation;

        isConfirmed = false;
    }

    public void SetEditStatus(bool isEditing)
    {
        this.isEditing = isEditing;
        Cursor.visible = !isEditing;
    }

    public void OnDeselect()
    {
        if (isEditing || !isConfirmed)
        {
            if(DeselectAction != null)
            {
                DeselectAction?.Invoke();
            }
            else
            {
                cachedView.transform.localPosition = cachedPosition;
                cachedView.transform.localRotation = cachedRotation;
                cachedView.Select(false);
                SetEditStatus(false);
            }
        }
    }

    public void Confirm()
    {
        if(isAvailable)
        {
            isConfirmed = true;
        }
    }

    private void Update()
    {
        EditPosition();
        Rotate();
    }

    private void Rotate()
    {
        if (Input.GetButtonDown("R") && isEditing)
        {
            Vector3 rotation = cachedView.transform.localRotation.eulerAngles;
            rotation.y += 90;

            cachedView.transform.localRotation = Quaternion.Euler(rotation);
            Core.Sound.PlayClip(AudioType.RotationSound);
        }
    }

    private void EditPosition()
    {
        if (isEditing)
        {
            if(editablePrefab != null)
            {
                float fixedHeight = 0;

                float distanceToObject = Vector3.Distance(topDownCamera.transform.position, parent.position);

                Vector3 mouseWorldPosition = topDownCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToObject));

                Vector3 localPosition = parent.InverseTransformPoint(mouseWorldPosition);

                localPosition.y = fixedHeight;

                editablePrefab.transform.localPosition = localPosition;

                isAvailable = cachedView.CheckAvailableToPlace();
                IsAvailablePosition?.Invoke(isAvailable);
            }
        }
    }
}
