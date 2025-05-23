using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShoppingCart : InteractiveManager, IDataProvider, IWindow
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject cartIcon;

    private const string KEY = "shopping_cart";

    private DataLoader<PositionData> dataLoader;
    private bool isAttached;

    public override void Interact()
    {
        isAttached = !isAttached;

        cartIcon.SetActive(isAttached);

        Core.Sound.PlayClip(AudioType.ShoppingCart);

        player.SetCartActivity(isAttached);

        Transform parent = isAttached ? player.CartPoint : null;

        transform.SetParent(parent);

        if(isAttached)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }

    private void Update()
    {
        if(isAttached)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Interact();
            }
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

    public void Close()
    {
        if (isAttached)
        {
            Interact();
        }
    }

    public bool IsActive()
    {
        return isAttached;
    }
}
