using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    private const string ANIMATOR_KEY = "fade";

    [SerializeField] private Animator loadingScreen;

    public void Appear() => loadingScreen.SetBool(ANIMATOR_KEY, true);

    public void Fade() => loadingScreen.SetBool(ANIMATOR_KEY, false);
}
