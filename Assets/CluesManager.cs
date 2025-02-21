using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CluesManager : MonoBehaviour
{
    [SerializeField] private CluesPanel[] clues;

    public static CluesManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ShowClue(string text)
    {
        for (int i = 0; i < clues.Length; i++)
        {
            if (!clues[i].gameObject.activeInHierarchy)
            {
                clues[i].ShowClue(text);
                return;
            }
        }
    }
}
