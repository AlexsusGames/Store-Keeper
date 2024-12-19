using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightTurner : InteractiveManager
{
    [SerializeField] private UnityEvent onLightChange;
    [SerializeField] private Vector3 turnerOnPosition;
    [SerializeField] private Vector3 turnerOffPosition;
    private bool isEnabled;
    public override void Interact()
    {
        isEnabled = !isEnabled;

        onLightChange?.Invoke();

        Vector3 euler = isEnabled ? turnerOnPosition : turnerOffPosition;
        transform.localRotation = Quaternion.Euler(euler);
    }
}
