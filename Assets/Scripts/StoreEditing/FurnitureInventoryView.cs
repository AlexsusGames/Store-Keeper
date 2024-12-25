using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FurnitureInventoryView : MonoBehaviour
{
    [SerializeField] private FurnitureSlotView[] slotsView;
    private StoreFurnitureConfigFinder configFinder;

    public void SetData(List<FurnitureData> data, UnityAction<string> action)
    {
        if(configFinder == null)
            configFinder = new StoreFurnitureConfigFinder();

        for (int i = 0; i < slotsView.Length; i++)
        {
            if (i < data.Count)
            {
                int index = i;
                var sprite = configFinder.FindByName(data[i].FurnitureId).shopSprite;
                slotsView[i].ItemId = data[i].FurnitureId;

                slotsView[i].SetData(sprite, data[i].Amount);
                slotsView[i].TryGetComponent(out Button button);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => action?.Invoke(slotsView[index].ItemId));
            }
            else
            {
                slotsView[i].Hide();
                slotsView[i].TryGetComponent(out Button button);
                button.onClick.RemoveAllListeners();
            }
        }
    }
}
