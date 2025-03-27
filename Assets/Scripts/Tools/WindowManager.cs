using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private GameObject[] windows;
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (TryCloseWindows() || Core.Camera.IsActive(CameraType.MainMenuCamera))
            {
                return;
            }

            else Core.Camera.SetCurrentCamera(CameraType.MainMenuCamera);
        }
    }

    private bool TryCloseWindows()
    {
        bool closed = false;

        for (int i = 0; i < windows.Length; i++)
        {
            if (windows[i].TryGetComponent(out IWindow window))
            {
                if(window.IsActive())
                {
                    window.Close();
                    closed = true;
                }
            }
            else
            {
                if (windows[i].activeInHierarchy)
                {
                    windows[i].SetActive(false);
                    closed = true;
                }
            }
        }

        return closed;
    }
}
