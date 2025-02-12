using UnityEngine;

public class InteractiveHandler : MonoBehaviour
{
    [SerializeField] private float rayDistance = 5f; 
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private ItemGrab itemGrabbing;

    [SerializeField] private GameInputView gameInputView;

    private Camera mainCamera;
    private IInteractable currentInteractable; 
    private bool isEnable = true;

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
        }
    }

    public void ChangeEnabled(bool value)
    {
        isEnable = value;
    }

    private void Update()
    {
        if (!isEnable) return;
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            if (currentInteractable is PickupObject obj)
                itemGrabbing.Grab(obj);
            currentInteractable.Interact();
        }
    }

    private void FixedUpdate()
    {
        HighlightObject();

        var inputClue = currentInteractable != null ? currentInteractable.GetInputClue() : null;
        gameInputView.AssignInteractableUnits(inputClue);
    }

    private void HighlightObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactableLayer))
        {

            if(hit.collider.TryGetComponent(out IInteractable interactable))
            {
                if(currentInteractable == interactable)
                {
                    return;
                }

                OnInteractableChange(interactable);
                return;
            }
        } else OnInteractableChange(null);
    }

    private void OnInteractableChange(IInteractable interactable)
    {
        if (currentInteractable != null)
        {
            if (currentInteractable is PickupObject obj)
            {
                obj.SetCapActive(true);
            }

            currentInteractable.OutlineEnabled(false);
            currentInteractable = null;
        }

        if(interactable != null)
        {
            currentInteractable = interactable;
            currentInteractable.OutlineEnabled(true);

            if (interactable is PickupObject obj)
            {
                obj.SetCapActive(false);
            }
        }
    }
}

