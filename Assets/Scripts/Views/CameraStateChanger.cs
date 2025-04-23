using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateChanger : MonoBehaviour
{
    [SerializeField] private CameraState[] states;

    private Coroutine currentCoroutine;

    public event Action<CameraType> StateChanged;

    private void Awake()
    {
        Core.Camera = this;
    }

    public void Init()
    {
        SetCurrentCamera(Core.StartCamera, true);
    }

    public bool IsActive(CameraType type) => states[(int)type].UI[0].enabled;

    public void BackToLastState() => SetCurrentCamera(Core.LastCamera);

    public void SetCurrentCamera(CameraType type, bool isInitial = false)
    {
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(ChangeState(type, isInitial));
        }

        else Debug.Log("State is already being changed");
    }
    private IEnumerator ChangeState(CameraType type, bool isInitial = false)
    {
        Action callback = () =>
        {
            StateChanged?.Invoke(type);

            int index = (int)type;

            for (int i = 0; i < states.Length; i++)
            {
                bool isCorrectState = index == i;
                ChangeStateEnabled(states[i], isCorrectState, isInitial);
            }

            ChangeStateEnabled(states[index], true, isInitial);


            currentCoroutine = null;
        };

        yield return FadeScreen.instance.Animation(callback, !isInitial);
    }

    private void ChangeStateEnabled(CameraState state, bool enabled, bool isInitial = false)
    {
        if (state.UI[0].enabled && !isInitial && state.Type != CameraType.MainMenuCamera)
            Core.LastCamera = state.Type;

        if (state.Root != null)
        {
            state.Root.SetActive(enabled);
        }

        for (int i = 0;i < state.UI.Length; i++)
        {
            state.UI[i].enabled = enabled;
        }
    }
}
[System.Serializable] 
public class CameraState
{
    public CameraType Type;
    public GameObject Root;
    public Canvas[] UI;
}

public enum CameraType
{
    GameplayCamera,
    StoreEditorCamera,
    MainMenuCamera
}
