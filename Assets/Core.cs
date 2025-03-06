using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Core 
{
    private static SoundManager soundManager;

    public static SoundManager Sound
    {
        get
        {
            if (soundManager == null)
            {
                soundManager = GameObject.FindObjectOfType<SoundManager>();
            }
            
            return soundManager;
        }
        set { soundManager = value; }
    }

    private static CluesManager cluesManager;

    public static CluesManager Clues
    {
        get
        {
            if (cluesManager == null)
            {
                cluesManager = GameObject.FindObjectOfType<CluesManager>();
            }

            return cluesManager;
        }

        set { cluesManager = value; }
    }
}
