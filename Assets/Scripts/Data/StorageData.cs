using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StorageData 
{
    public string ProductName;
    public int ProductCount;
    public float ProductWeight;
    public bool IsSpoilt;

    public string StorageId;

    public Vector3 Position;
    public Quaternion Rotation;

    public List<StorageData> Childs;
}
