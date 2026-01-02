using System;

public static class OfficeStateMachine
{
    public static OfficeState currentState { get; private set; } = OfficeState.BeforeStart;

    public static event Action<OfficeState> OnStateChanged;

    public static void SetState(OfficeState next)
    {   
        if (currentState == next) return;

        currentState = next;
        OnStateChanged?.Invoke(next);
    }
}
