using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fonts : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset[] interfaceFonts;

    public TMP_FontAsset GetByIndex(int index)
    {
        return interfaceFonts[index];
    }
}
