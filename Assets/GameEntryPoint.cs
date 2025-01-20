using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private StoreEditor storeEditor;
    [SerializeField] private Player player;

    private void Start()
    {
        storeEditor.Load();
        player.Load();
    }

    private void OnDisable()
    {
        storeEditor.Save();
        player.Save();
    }
}
