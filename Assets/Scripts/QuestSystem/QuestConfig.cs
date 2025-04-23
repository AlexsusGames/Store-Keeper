using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/QuestConfig", fileName = "QuestConfig")]
public class QuestConfig : ScriptableObject
{
    public Sprite Icon;
    public string Title;
    public string Description;
    public string Id;
    public int AimAmount;
}
