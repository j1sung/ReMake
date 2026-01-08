using System;
using System.Diagnostics;

public static class OfficeStateMachine
{
    public static OfficeState currentState { get; private set; } = OfficeState.Intro;

    public static event Action<OfficeState> OnStateChanged;

    public static void SetState(OfficeState next)
    {   
        if (currentState == next) return;

        currentState = next;
        OnStateChanged?.Invoke(next);
        UnityEngine.Debug.Log("현재 상태" + currentState);

        // Calling 도중에는 저장 x
        if (currentState == OfficeState.Calling || currentState == OfficeState.BeforeCall) return;
        DataManager.Instance.SaveGame();
    }
}
