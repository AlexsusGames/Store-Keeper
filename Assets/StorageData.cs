using UnityEngine;

[System.Serializable]
public class StorageData 
{
    public string ProductName;
    public int ProductCount;

    public string StorageId;

    public Vector3 Position;
    public Quaternion Rotation;

    public StorageData Child;
}
