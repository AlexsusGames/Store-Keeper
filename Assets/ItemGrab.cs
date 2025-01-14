using UnityEngine;

public class ItemGrab : MonoBehaviour
{
    [SerializeField] private Transform grabPoint;
    [SerializeField] private float grabDistance;
    [SerializeField] private int grabLayerMask;
    [SerializeField] private int interactableLayerMask;
    [SerializeField] private LayerMask surfaceMask;

    [SerializeField] private InteractiveHandler interactableHandler;
    [SerializeField] private GameInputView gameInputView;
    [SerializeField] private StoreEditor storeEditor;

    private PickupObject grabbedItem;
    private Quaternion standartRotation = Quaternion.Euler(Vector3.zero);
    private Camera mCamera;

    private bool editMode;

    private void Awake()
    {
        mCamera = Camera.main;
    }

    private void Start()
    {
        var savedObj = storeEditor.GetGrabbedSavedObject();

        if (savedObj != null)
        {
            Grab(savedObj);
        }
    }

    private void Update()
    {
        if(grabbedItem != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                editMode = !editMode;
            }

            if (!editMode)
            {
                SetStandartPosition();
                gameInputView.SetItemGrabbedState();
            }
        }
        else gameInputView.SetEmptyHandState();

        if (editMode)
        {
            gameInputView.SetEditModeState();
            if (grabbedItem != null && Input.GetButtonDown("R"))
            {
                Vector3 currentRotation = standartRotation.eulerAngles;
                currentRotation.y += 90f;

                standartRotation = Quaternion.Euler(currentRotation);
            }

            if (grabbedItem != null)
            {
                Ray ray = mCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

                if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, surfaceMask))
                {
                    if (hit.collider.TryGetComponent(out StoreBoxRef box))
                    {
                        SetChildPosition(box.StoreBox.ChildPoint);

                        HundleClick();

                        return;
                    }

                    StorageSurface storeSurface = null;

                    if (hit.collider.TryGetComponent(out StorageSurface storage))
                    {
                        storeSurface = storage;

                        if (!storage.IsOpened())
                        {
                            SetStandartPosition();
                            return;
                        }
                    }

                    if (hit.collider.tag == "surface")
                    {
                        SetCustomPosition(hit.transform, hit.point);

                        HundleClick(storeSurface);

                        return;
                    }
                }

                SetStandartPosition();
            }
        }
    }

    private void HundleClick(StorageSurface storeSurface = null)
    {
        bool isAvailable = grabbedItem.CheckAvailableToPlace();

        if (isAvailable && Input.GetMouseButtonDown(0))
        {
            grabbedItem.Place();
            editMode = false;
            grabbedItem = null;
        }
    }

    private void SetStandartPosition()
    {
        grabbedItem.transform.SetParent(grabPoint.transform);
        grabbedItem.transform.localPosition = Vector3.zero;
        grabbedItem.transform.localRotation = Quaternion.identity;

        grabbedItem.SetDefaut();

        grabbedItem.ChangeLayer(grabLayerMask);
    }

    private void SetCustomPosition(Transform parent, Vector3 position)
    {
        grabbedItem.transform.SetParent(parent);
        grabbedItem.ChangeLayer(interactableLayerMask);

        grabbedItem.transform.position = position;
        grabbedItem.transform.localRotation = standartRotation;
    }

    private void SetChildPosition(Transform parent)
    {
        grabbedItem.transform.SetParent(parent);
        grabbedItem.ChangeLayer(interactableLayerMask);

        grabbedItem.transform.localPosition = Vector3.zero;
        grabbedItem.transform.localRotation = standartRotation;
    }

    public void GetRidOfEmptyBox()
    {
        if (grabbedItem != null)
        {
            if(grabbedItem.TryGetComponent(out StoreBox box))
            {
                if(box.GetItemsAmount() == 0)
                {
                    Destroy(grabbedItem.gameObject);
                    grabbedItem = null;
                    editMode = false;
                }
            }
        }
    }

    public void Grab(PickupObject obj)
    {
        if(grabbedItem == null)
        {
            if(obj.TryGetComponent(out IOverheadChecker checker))
            {
                grabbedItem = obj;
                grabbedItem.Grab();

                if (!checker.IsAvailableToGrab())
                {
                    if(checker is StoreBox box)
                    {
                        if (!box.IsHasChild)
                        {
                            SetStandartPosition();
                            return;
                        }
                    }

                    grabbedItem.Place();
                    grabbedItem = null;
                    return;
                }

                SetStandartPosition();
            }
        }

        else
        {
            if(grabbedItem.gameObject.TryGetComponent(out Box grabbedBox) && obj.TryGetComponent(out Box box))
            {
                if (grabbedBox.GetType() == box.GetType())
                {
                    if(grabbedBox.GetItemsAmount() > 0 && box.CanTake() > 0)
                    {
                        var itemsToReplace = 1;

                        grabbedBox.RemoveItems(itemsToReplace);
                        box.AddItems(itemsToReplace);
                    }
                }
            }
        }
    }
}
