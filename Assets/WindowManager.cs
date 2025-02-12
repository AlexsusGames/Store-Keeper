using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private GameObject[] windows;
    private void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            for (int i = 0; i < windows.Length; i++)
            {
                if (windows[i].TryGetComponent(out IWindow window))
                {
                    window.Close();
                }
                else windows[i].SetActive(false);
            }
        }
    }
}
