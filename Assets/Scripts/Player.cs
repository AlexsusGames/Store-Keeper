using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private FirstPersonCamera playerCamera;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private InteractiveHandler interactiveHandler;

    public FirstPersonCamera FirstPersonCamera => playerCamera;
    public FirstPersonController PlayerController => playerController;
    public InteractiveHandler InteractiveHandler => interactiveHandler;
}
