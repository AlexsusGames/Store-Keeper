using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tablet : InteractiveManager
{
    [SerializeField] private GameObject view;

    [SerializeField] private GameObject clipboardIcon;

    private bool isEnable;

    private void Awake()
    {
        isEnable = false;
        ChangeClipboardEnabled(false);
    }


    public void ChangeClipboardEnabled(bool value)
    {
        gameObject.SetActive(value);
        clipboardIcon.SetActive(value);

        if (value)
        {
            PlayTabletSound();
        }
    }

    public void PlayTabletSound()
    {
        Core.Sound.PlayClip(AudioType.Tablet);
    }

    public override void Interact()
    {
        isEnable = view.activeInHierarchy;

        isEnable = !isEnable;

        view.SetActive(isEnable);
    }
}
