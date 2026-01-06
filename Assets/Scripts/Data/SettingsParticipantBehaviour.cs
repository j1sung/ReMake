using System.Collections;
using UnityEngine;

public abstract class SettingsParticipantBehaviour : MonoBehaviour, ISettingsParticipant
{
    public virtual int Order => 0;

    protected virtual void OnEnable()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.RegisterSettings(this);
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
        DataManager.Instance.RegisterSettings(this);
    }

    protected virtual void OnDisable()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.UnRegisterSettings(this);
        }
    }

    public abstract void Capture(SettingsData data);

    public abstract void Apply(SettingsData data);
}
