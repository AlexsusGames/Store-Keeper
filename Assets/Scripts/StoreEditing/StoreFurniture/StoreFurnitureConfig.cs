using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/FurnitureConfig", fileName = "FurnitureConfig")]
public class StoreFurnitureConfig : ScriptableObject
{
    public string id;
    public Sprite shopSprite;
    public GameObject prefab;
    public int Price;
}