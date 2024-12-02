using UnityEngine;

public class InteractiveHandler : MonoBehaviour
{
    [SerializeField] private float rayDistance = 5f; 
    [SerializeField] private LayerMask interactableLayer; 

    private Camera mainCamera;
    private IInteractable currentInteractable; 

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
        }
    }

    private void Update()
    {
        HighlightObject();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
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

                currentInteractable = interactable;
                currentInteractable.OutlineEnabled(true);
                return;
            }
        }

        if(currentInteractable != null)
        {
            currentInteractable.OutlineEnabled(false); 
            currentInteractable = null;
        }
    }
}

