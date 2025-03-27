using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/MessageConfig", fileName = "MessageConfig")]
public class MessageConfig : ScriptableObject
{
    public bool IsPlayer;
    public MessageContent Content;
}

[System.Serializable]
public class MessageContent
{
    public Sprite Sprite;
    public string Message;
}
