using System.Collections;
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

    [SerializeField] private BoxInfoView view;

    private PickupObject grabbedItem;
    private Quaternion standartRotation = Quaternion.Euler(Vector3.zero);
    private Camera mCamera;

    private bool editMode;
    public bool IsEnabled { get; set; }

    private void Awake()
    {
        mCamera = Camera.main;
        IsEnabled = true;
    }

    private IEnumerator Start()
    {
        yield return null;

        if(grabPoint.childCount > 0)
        {
            grabPoint.GetChild(0).TryGetComponent(out PickupObject savedObj);

            if (savedObj != null)
            {
                Grab(savedObj);
            }
        }
    }

    private void Update()
    {
        if(!IsEnabled)
        {
            return;
        }

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
        else gameInputView.SetNullState();

        if (editMode)
        {
            gameInputView.SetItemEditingState();
            if (grabbedItem != null && Input.GetButtonDown("R"))
            {
                Vector3 currentRotation = standartRotation.eulerAngles;
                currentRotation.y += 90f;

                standartRotation = Quaternion.Euler(currentRotation);
                Core.Sound.PlayClip(AudioType.RotationSound);
            }

            if (grabbedItem != null)
            {
                Ray ray = mCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

                if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, surfaceMask))
                {
                    if (hit.collider.TryGetComponent(out StoreBoxRef box))
                    {
                        bool canFold = true;

                        if (box.StoreBox.FoldBtType)
                        {
                            if(grabbedItem.StoreBox.BoxType != box.StoreBox.BoxType)
                            {
                                canFold = false;
                            }
                        }

                        if(canFold)
                        {
                            if (grabbedItem.IsFreeToPlace)
                            {
                                SetCustomPosition(box.StoreBox.ChildPoint, hit.point);
                            }
                            else SetChildPosition(box.StoreBox.ChildPoint);

                            HundleClick();

                            return;
                        }
                    }

                    if (hit.collider.TryGetComponent(out StorageSurface storage))
                    {
                        if (!storage.IsOpened())
                        {
                            SetStandartPosition();
                            return;
                        }
                    }

                    if (hit.collider.tag == "surface")
                    {
                        SetCustomPosition(hit.transform, hit.point);

                        HundleClick();

                        return;
                    }
                }

                SetStandartPosition();
            }
        }
    }

    private void HundleClick()
    {
        bool isAvailable = grabbedItem.CheckAvailableToPlace();

        if (Input.GetMouseButtonDown(0))
        {
            if (isAvailable)
            {
                grabbedItem.Place();
                view.Hide();
                editMode = false;
                grabbedItem = null;

                Core.Sound.PlayClip(AudioType.BoxFold);
            }

            else Core.Clues.Show("A drawer cannot be placed here.");
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
                string item = box.ProductName;
                int amount = box.GetItemsAmount();

                if(amount == 0 || box.IsSpoilt)
                {
                    Destroy(grabbedItem.gameObject);
                    grabbedItem = null;
                    editMode = false;

                    Core.ProductList.RemoveProduct(item, amount);

                    view.Hide();
                }
            }
        }
    }

    public void Grab(PickupObject obj)
    {
        if(!IsEnabled)
        {
            return;
        }

        if(grabbedItem == null)
        {
            if(obj.TryGetComponent(out StoreBox box))
            {
                if (!box.IsHasChild)
                {
                    grabbedItem = obj;
                    grabbedItem.Grab();

                    SetStandartPosition();
                    view.ShowInfo(box);

                    Core.Sound.PlayClip(AudioType.Grab);
                }
                else Core.Clues.Show("First, remove the top drawer");
            }
        }

        else
        {
            if(grabbedItem.gameObject.TryGetComponent(out StoreBox grabbedBox) && obj.TryGetComponent(out StoreBox box))
            {
                if (grabbedBox.ProductName == box.ProductName)
                {
                    if(grabbedBox.GetItemsAmount() > 0 && box.CanTake() > 0)
                    {
                        if (!box.IsSpoilt)
                        {
                            var itemsToReplace = 1;

                            grabbedBox.RemoveItems(itemsToReplace);
                            box.AddItems(itemsToReplace);

                            view.ShowInfo(grabbedBox);

                            Core.Sound.PlayClip(AudioType.Transfer);
                        }
                        else Core.Clues.Show("You can't transfer a product into a box of spoiled goods.");
                    }
                }

                else Core.Clues.Show("Products can only be transferred between identical drawers");
            }
        }
    }
}
