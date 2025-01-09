using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FurnitureSelectionEditor : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private Camera topDownCamera;
    [SerializeField] private float rayDistance = 200f;

    private FurniturePlacementView cachedView;
    private bool isSelected;
    public bool IsSelected => isSelected;

    public event Action<FurniturePlacementView> OnSelected;
    public event Action OnSelectionCanceled;
    public event Action OnConfirmed;

    public bool IsCreated;

    private void Update()
    {
        TrySelectFurniture();
        HandleMouseClick();
    }

    private void OnDisable()
    {
        Deselect();
    }

    public void Select(FurniturePlacementView view)
    {
        isSelected = true;
        view.Select(true);

        cachedView = view;
        IsCreated = true;

        OnSelected?.Invoke(view);
    }

    private void TrySelectFurniture()
    {
        if (!isSelected)
        {
            Ray ray = topDownCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, layer))
            {
                if (hit.collider.TryGetComponent(out FurniturePlacementView furniture))
                {
                    if (cachedView == furniture)
                    {
                        return;
                    }
                    else if (cachedView != null)
                    {
                        cachedView.Select(false);
                    }

                    cachedView = furniture;
                    furniture.Select(true);
                    return;
                }
            }

            if (cachedView != null)
            {
                cachedView.Select(false);
                cachedView = null;
            }
        }
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(isSelected)
            {
                OnConfirmed?.Invoke();
                return;
            }

            if (cachedView != null)
            {
                isSelected = true;

                cachedView.ChangeOutlineColor(Color.green);
                IsCreated = false;

                OnSelected?.Invoke(cachedView);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Deselect();
        }
    }

    public void Deselect()
    {
        if(cachedView != null)
        {
            cachedView.Select(false);

            cachedView = null;

            isSelected = false;

            OnSelectionCanceled?.Invoke();
        }
    }
}
