using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShoppingCart : InteractiveManager, IDataProvider
{
    [SerializeField] private Player player;

    private const string KEY = "shopping_cart";

    private DataLoader<PositionData> dataLoader;
    private bool isAttached;

    public override void Interact()
    {
        isAttached = !isAttached;

        Transform parent = isAttached ? player.CartPoint : null;

        player.FirstPersonCamera.SetCartEnabled(isAttached);
        transform.SetParent(parent);

        if(isAttached)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

    }

    public void Load()
    {
        dataLoader = new();

        var data = dataLoader.Load(KEY);

        if(data != null)
        {
            transform.position = data.Position;
            transform.rotation = data.Rotation;
        }
    }

    public void Save()
    {
        PositionData data = new();
        data.Position = transform.position;
        data.Rotation = transform.rotation;

        dataLoader.Save(data, KEY);
    }
}
