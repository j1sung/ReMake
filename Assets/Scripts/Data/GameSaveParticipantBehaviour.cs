using System.Collections;
using UnityEngine;

public abstract class GameSaveParticipantBehaviour : MonoBehaviour, IGameSaveParticipant
{
    public virtual int Order => 0;

    public abstract ApplyPhase Phase { get; }

    protected virtual void OnEnable()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.RegisterGame(this);
        }
        else
        {
            StartCoroutine(RegisterWhenReady());
        }
    }
    
    private IEnumerator RegisterWhenReady()
    {
        while (DataManager.Instance == null)
            yield return null;
        DataManager.Instance.RegisterGame(this);
    }

    protected virtual void OnDisable()
    {
        if(DataManager.Instance != null)
        {
            DataManager.Instance.UnRegisterGame(this);
        }
    }
    public abstract void Capture(GameSaveData data);

    public abstract void Apply(GameSaveData data);
}
