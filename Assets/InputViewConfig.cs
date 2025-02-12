using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/InputView", fileName = "InputView")]
public class InputViewConfig : ScriptableObject
{
    public Sprite InputSprite;
    public string InputName;

    public bool isIgnored;
}
