using System.Collections;
using System.Collections.Generic;
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
        Interact();
    }

    public override void Interact()
    {
        value = !value;

        Quaternion rotation = value ? angleRotation : currentRotation;

        transform.localRotation = rotation;
    }
}
