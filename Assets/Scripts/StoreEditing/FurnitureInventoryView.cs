using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureInventoryView : MonoBehaviour
{
    [SerializeField] private FurnitureSlotView[] slotsView;
    private StoreFurnitureConfigFinder configFinder;

    public void SetData(List<FurnitureData> data)
    {
        if(configFinder == null)
            configFinder = new StoreFurnitureConfigFinder();

        for (int i = 0; i < slotsView.Length; i++)
        {
            if (i < data.Count)
            {
                var sprite = configFinder.FindById(data[i].FurnitureId).shopSprite;

                slotsView[i].SetData(sprite, data[i].Amount);
            }
            else slotsView[i].Hide();
        }
    }
}
