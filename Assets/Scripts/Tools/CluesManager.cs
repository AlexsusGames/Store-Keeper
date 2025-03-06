using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CluesManager : MonoBehaviour
{
    [SerializeField] private CluesPanel[] clues;

    private void Awake() => Core.Clues = this;

    public void Show(string text)
    {
        for (int i = 0; i < clues.Length; i++)
        {
            if (!clues[i].gameObject.activeInHierarchy)
            {
                Core.Sound.PlayClip(AudioType.UIAppearence);
                clues[i].ShowClue(text);
                return;
            }
        }
    }
}
