using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    private const string ANIMATOR_KEY = "fade";
    public static FadeScreen instance;

    private Coroutine coroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    [SerializeField] private Animator loadingScreen;

    public bool TryShowLoadingScreen(Action callback)
    {
        if(coroutine ==  null)
        {
            callback += () => coroutine = null;

            coroutine = StartCoroutine(Animation(callback));
            return true;
        }

        return false;
    }

    public IEnumerator Animation(Action callback, bool animate = true)
    {
        if(animate)
        {
            loadingScreen.SetBool(ANIMATOR_KEY, true);

            yield return new WaitForSeconds(0.5f);

            callback?.Invoke();

            loadingScreen.SetBool(ANIMATOR_KEY, false);

            yield break;
        }

        yield return null;

        callback?.Invoke();
    }
}
