using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Dialog", fileName = "Dialog")]
public class DialogConfig : ScriptableObject
{
    public string CallerName;
    public string FirstAction;
    public string SecondAction;

    public Sprite CallerSprite;

    public MessageConfig[] Messages;
}
