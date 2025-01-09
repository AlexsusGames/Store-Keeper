using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBoxRef : MonoBehaviour
{
    [SerializeField] private StoreBox box;
    public StoreBox StoreBox => box;
}
