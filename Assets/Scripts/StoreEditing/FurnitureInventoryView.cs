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

        ResetViews();

        for (int i = 0; i < data.Count; i++)
        {
            int index = i;
            var config = configFinder.FindByName(data[i].FurnitureId);

            if (config.isStatic)
                continue;

            var sprite = config.shopSprite;

            var view = GetFreeSlot();

            view.ItemId = data[i].FurnitureId;

            view.SetData(sprite, data[i].Amount);
            view.TryGetComponent(out Button button);

            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(() => action?.Invoke(view.ItemId));
        }
    }

    private FurnitureSlotView GetFreeSlot()
    {
        for (int i = 0;i < slotsView.Length;i++)
        {
            if (!slotsView[i].Enabled) return slotsView[i];
        }

        throw new System.Exception("There is no free storage view");
    }

    private void ResetViews()
    {
        for (int i = 0;i < slotsView.Length; i++)
        {
            slotsView[i].Hide();
            slotsView[i].TryGetComponent(out Button button);
            button.onClick.RemoveAllListeners();
        }
    }
}
