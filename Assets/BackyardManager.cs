using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackyardManager : MonoBehaviour, IDataProvider
{
    [SerializeField] private Animator gateAnimator;
    [SerializeField] private Animator truckAnimator;
    [SerializeField] private TruckView truckView;

    private const string KEY = "DeliverDataSave";
    private DeliverData data;

    private CarType currentCar;
    private bool isDelivering;

    private void Start()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        OpenGates(true);

        CarType type = CarType.Grocceries;

        truckView.ChangeSkin(type);

        SetCarDrivingAnimation(true);

        yield return new WaitForSeconds(2);
    }

    private void OpenGates(bool value)
    {
        gateAnimator.SetBool("isOpen", value);
    }

    public void DeliverProducts()
    {
        SetCarDrivingAnimation(true);
        isDelivering = true;
    }

    public void OnDelivered()
    {
        SetCarDrivingAnimation(false);
        isDelivering = false;
    }

    private void SetCarDrivingAnimation(bool value)
    {
        truckAnimator.SetBool("drivingInside", value);
    }

    public void Save()
    {
        DeliverData data = new DeliverData()
        {
            IsDeliviring = isDelivering,
            CarType = currentCar
        };

        string save = JsonUtility.ToJson(data);

        PlayerPrefs.SetString(KEY, save);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            data = JsonUtility.FromJson<DeliverData>(save);

            if(data.IsDeliviring)
            {
                isDelivering = true;
                truckView.ChangeSkin(data.CarType);
                truckView.SetBeckyardPosition();
            }
        }
    }
}

[System.Serializable]
public class DeliverData
{
    public CarType CarType;
    public bool IsDeliviring;
}
