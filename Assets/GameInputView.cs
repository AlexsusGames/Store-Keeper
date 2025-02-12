using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputView : MonoBehaviour
{
    [SerializeField] private InputUnitView[] views;

    [SerializeField] private InputViewConfig[] itemGrabState;
    [SerializeField] private InputViewConfig[] itemEditState;

    private event Action<InputViewConfig> configChanged;

    private InputViewConfig[] cathedInteractable;
    private InputViewConfig[] cathedState;

    private void Awake()
    {
        for (int i = 0; i < views.Length; i++)
        {
            configChanged += views[i].DisableViewByConfig;
        }
    }
    public void SetItemGrabbedState() => AssignStateUnits(itemGrabState);
    public void SetItemEditingState() => AssignStateUnits(itemEditState);
    public void SetNullState() => AssignStateUnits(null);

    private void AssignStateUnits(InputViewConfig[] configs) => AssignUnits(configs, ref cathedState);
    public void AssignInteractableUnits(InputViewConfig[] configs) => AssignUnits(configs, ref cathedInteractable);

    private void AssignUnits(InputViewConfig[] configs, ref InputViewConfig[] cachedConfigs)
    {
        if (configs == cachedConfigs)
            return;

        if (configs == null)
        {
            ExchangeConfigs(cachedConfigs);
            cachedConfigs = configs;
            return;
        }

        ExchangeConfigs(cachedConfigs);
        PlaceNewClues(configs);

        cachedConfigs = configs;
    }

    private void PlaceNewClues(InputViewConfig[] configs)
    {
        for(int i = 0;i < configs.Length; i++)
        {
            if (configs[i].isIgnored && cathedState != null)
                continue;

            var freeUnit = GetFreeView();
            freeUnit.SetData(configs[i]);
        }
    }

    private InputUnitView GetFreeView()
    {
        for(int i = 0;i < views.Length;i++)
        {
            if (!views[i].gameObject.activeInHierarchy)
            {
                return views[i];
            }
        }

        throw new Exception("All inputViews are busy now");
    }

    private void ExchangeConfigs(InputViewConfig[] configs)
    {
        if(configs != null)
        {
            for (int i = 0; i < configs.Length; i++)
            {
                configChanged?.Invoke(configs[i]);
            }
        }
    }
}
