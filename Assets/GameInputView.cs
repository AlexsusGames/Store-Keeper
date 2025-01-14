using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputView : MonoBehaviour
{
    [SerializeField] private RectTransform[] points;

    [SerializeField] private RectTransform[] emptyHandState;
    [SerializeField] private RectTransform[] itemGrabbedState;
    [SerializeField] private RectTransform[] editModeState;

    private List<GameObject> currentState;
    private int currentStateIndex;

    public void AssignEmptyState(RectTransform[] state)
    {
        emptyHandState = state;

        if(currentStateIndex == 1)
        {
            if (state == null)
            {
                SetNullState();
                return;
            }

            currentStateIndex = 0;
        }
    }

    private void SetNullState() => SetState(null, 4);
    public void SetEmptyHandState() => SetState(emptyHandState, 1);
    public void SetItemGrabbedState() => SetState(itemGrabbedState, 2);
    public void SetEditModeState() => SetState(editModeState, 3);

    private void RemoveLastState()
    {
        if(currentState != null)
        {
            for (int i = 0; i < currentState.Count; i++)
            {
                Destroy(currentState[i].gameObject);
            }
        }

        currentState = new();
    }

    private void SetState(RectTransform[] state, int index)
    {
        if(currentStateIndex != index)
        {
            RemoveLastState();

            if (state != null)
            {
                for (int i = 0; i < state.Length; i++)
                {
                    var item = Instantiate(state[i], points[i]);
                    currentState.Add(item.gameObject);
                }
            }

            currentStateIndex = index;
        }
    }
}
