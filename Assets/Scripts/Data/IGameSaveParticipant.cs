using UnityEngine;

public enum ApplyPhase
{
    BeforeSceneLoad,
    AfterSceneLoad
}


public interface IGameSaveParticipant
{
    int Order { get; }
    ApplyPhase Phase {  get; }

    void Capture(GameSaveData data);
    void Apply(GameSaveData data);
    
}
