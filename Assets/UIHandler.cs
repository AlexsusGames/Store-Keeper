using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private PhoneController phoneController;

    private void Update()
    {
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
