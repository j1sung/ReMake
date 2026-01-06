using UnityEngine;

public interface ISettingsParticipant
{
    int Order { get; }
    void Capture(SettingsData data);
    void Apply(SettingsData data);
}
