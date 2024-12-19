using UnityEngine;
using UnityEngine.Events;

public class FurniturePositionEditor : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private Camera topDownCamera;
    [SerializeField] private float rayDistance = 50f;

    [SerializeField] private FurnitureHandlerMenu handleMenu;
    [SerializeField] private StoreEditor storeEditor;

    private FurniturePlacementView cachedView;
    private StoreFurnitureConfigFinder configFinder;
    private FurnitureFactory furnitureFactory;

    private bool isEditing;

    private void Awake()
    {
        configFinder = new();
    }

    private void Update()
    {
        TrySelectFurniture();
        HandleMouseClick();
    }

    private void HandleMouseClick()
    {
        if(!isEditing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (cachedView != null)
                {
                    isEditing = true;
                    var sprite = configFinder.FindById(cachedView.FurnitureId).shopSprite;

                    UnityAction storeAction = () =>
                    {
                        storeEditor.StoreFurniture(cachedView.FurnitureId);
                        Destroy(cachedView.gameObject);
                        isEditing = false;
                    };

                    UnityAction confirmAction = () =>
                    {
                        isEditing = false;
                    };

                    handleMenu.SetFurniture(sprite, storeAction, confirmAction, null);
                }
            }
        }
    }

    private void TrySelectFurniture()
    {
        if(!isEditing)
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
                        cachedView.Choose(false);
                    }

                    cachedView = furniture;
                    furniture.Choose(true);
                    return;
                }
            }

            if (cachedView != null)
            {
                cachedView.Choose(false);
                cachedView = null;
            }
        }
    }
}
