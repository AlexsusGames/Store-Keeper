using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableDisableActions : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnableAction;
    [SerializeField] private UnityEvent onDisableAction;

    private void OnEnable() => onEnableAction?.Invoke();
    private void OnDisable() => onDisableAction?.Invoke();
}
