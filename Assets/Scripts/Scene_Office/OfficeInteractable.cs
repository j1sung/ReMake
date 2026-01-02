using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public abstract class OfficeInteractable : MonoBehaviour, IPointerClickHandler
{
    protected Dictionary<OfficeState, Action> actions;

    public void OnPointerClick(PointerEventData eventData)
    {
        Interact();
    }

    public void Interact()
    {
        var state = OfficeStateMachine.currentState;

        if (actions != null && actions.TryGetValue(state, out var action))
            action.Invoke();
        else
            OnInvalid(state);
    }

    protected virtual void OnInvalid(OfficeState state)
    {
        Debug.Log("동작 없음");
    }
}