using UnityEngine;

public class OfficeStateMachine : MonoBehaviour
{
    public static OfficeStateMachine Instance;
    public OfficeState currentState;

    public event System.Action<OfficeState> OnStateChanged;

    void Awake()
    {   
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        OnStateChanged = null;  
        currentState = OfficeState.BeforeStart;
    }

    public void SetState(OfficeState next)
    {   
        if (currentState == next) return;

        currentState = next;
        OnStateChanged?.Invoke(next);
    }
}
