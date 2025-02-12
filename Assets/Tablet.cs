using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : InteractiveManager
{
    [SerializeField] private GameObject view;

    private bool isEnable;

    private void Awake()
    {
        isEnable = false;
    }

    public override void Interact()
    {
        isEnable = view.activeInHierarchy;

        isEnable = !isEnable;

        view.SetActive(isEnable);
    }
}
