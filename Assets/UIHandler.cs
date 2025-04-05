using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private PhoneController phoneController;

    private bool isBlocked;

    public void BlockEnabled(bool value)
    {
        isBlocked = value;
    }

    private void Update()
    {
        if (isBlocked) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            phoneController.TryGetComponent(out IWindow window);

            if (!window.IsActive() && !phoneController.gameObject.activeInHierarchy)
                phoneController.OpenMessenger();
            else 
                window.Close();
        }
    }
}
