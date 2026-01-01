using UnityEngine;

public class ResultSaveParticipant : GameSaveParticipantBehaviour
{
    public override int Order => 4;
    public override ApplyPhase Phase => ApplyPhase.BeforeSceneLoad;

    public override void Apply(GameSaveData data)
    {
        
    }

    public override void Capture(GameSaveData data)
    {
        
    }
}
