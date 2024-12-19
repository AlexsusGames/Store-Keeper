using UnityEngine;

public class StoreFurnitureConfigFinder 
{
    private StoreFurnitureConfig[] storeFurnitureConfigs;

    public StoreFurnitureConfigFinder()
    {
        storeFurnitureConfigs = Resources.LoadAll<StoreFurnitureConfig>("StoreFurnitureConfigs");
    }

    public StoreFurnitureConfig FindById(string id)
    {
        foreach(var config in storeFurnitureConfigs)
        {
            if(config.id == id) 
                return config;
        }

        throw new System.Exception($"There is no such a config with id: {id}");
    }
}
