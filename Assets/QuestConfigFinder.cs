using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestConfigFinder 
{
    private Dictionary<string, QuestConfig> questMap;

    private void CreateMap()
    {
        var configs = Resources.LoadAll<QuestConfig>("Quests");
        questMap = new Dictionary<string, QuestConfig>();

        for (int i = 0; i < configs.Length; i++)
        {
            var id = configs[i].Id;
            var config = configs[i];

            questMap[id] = config;
        }
    }

    public QuestConfig FindById(string id)
    {
        if(questMap ==  null)
            CreateMap();

        return questMap[id];
    }
}
