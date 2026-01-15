using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public abstract class OfficeInteractable : MonoBehaviour, IPointerClickHandler
{
    protected Dictionary<OfficeState, Action> actions;

    [Header("SFX")]
    [SerializeField] private AudioClip interactSFX; // 사물 고유 효과음
    public void OnPointerClick(PointerEventData eventData)
    {
        Interact();
    }

    public void Interact()
    {
        var state = OfficeStateMachine.currentState;
        if (state == OfficeState.Calling) return;
        if (actions != null && actions.TryGetValue(state, out var action))
        {
            PlayInteractSFX();
            action.Invoke();
        }

        else
            OnInvalid(state);
    }

    protected virtual void OnInvalid(OfficeState state)
    {
        Debug.Log("동작 없음");
    }

    protected virtual void PlayInteractSFX()
    {
        if (interactSFX == null) return;
        SFXPlayer.Instance?.PlaySFX(interactSFX);
    }
}