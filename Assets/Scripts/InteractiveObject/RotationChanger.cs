using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class RotationChanger : InteractiveManager
{
    [SerializeField] private Vector3 angleVector;

    private bool value;

    private Quaternion angleRotation => Quaternion.Euler(angleVector);
    private Quaternion currentRotation;

    private void Awake()
    {
        currentRotation = transform.localRotation;
    }

    public void SetStandart()
    {
        value = true;
        ChangeRotation();
    }

    public override void Interact()
    {
        ChangeRotation(true);
    }

    private void ChangeRotation(bool soundEnabled = false)
    {
        value = !value;

        Quaternion rotation = value ? angleRotation : currentRotation;

        transform.localRotation = rotation;

        if(soundEnabled)
        {
            Core.Sound.PlayClip(AudioType.Door);
        }
    }
}
