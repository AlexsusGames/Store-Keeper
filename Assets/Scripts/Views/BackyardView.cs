using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BackyardView : MonoBehaviour
{
    [SerializeField] private Animator gateAnimator;
    [SerializeField] private Animator truckAnimator;
    [SerializeField] private TruckView truckView;

    [SerializeField] private DeliveringManager manager;

    private void Awake()
    {
        manager.OnCarArrived += OnCarArrived;
        manager.OnCarDelivered += OnFinishDelivery;
    }

    public void OnCarArrived(CarType carType)
    {
        truckView.ChangeSkin(carType);
        OpenGates(true);

        SetCarDrivingAnimation(true);
    }

    public void OnFinishDelivery()
    {
        SetCarDrivingAnimation(false);

        OpenGates(false);
    }

    private void OpenGates(bool value)
    {
        gateAnimator.SetBool("isOpen", value);
    }

    private void SetCarDrivingAnimation(bool value)
    {
        truckAnimator.SetBool("drivingInside", value);
        truckView.PlaySound();
    }
}
