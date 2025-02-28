using UnityEngine;

public class StoreFurnitureConfigFinder 
{
    private StoreFurnitureConfig[] storeFurnitureConfigs;

    public StoreFurnitureConfigFinder()
    {
        storeFurnitureConfigs = Resources.LoadAll<StoreFurnitureConfig>("StoreFurnitureConfigs");
    }

    public StoreFurnitureConfig FindByName(string name)
    {
        foreach(var config in storeFurnitureConfigs)
        {
            if(config.Name == name) 
                return config;
        }

        throw new System.Exception($"There is no such a config with id: {name}");
    }
}
