using UnityEngine;

public class OfficeSaveParticipant : GameSaveParticipantBehaviour
{
    public override int Order => 3;
    public override ApplyPhase Phase => ApplyPhase.BeforeSceneLoad;

    public override void Capture(GameSaveData data)
    { 
        data.officeState = OfficeStateMachine.currentState;
    }

    public override void Apply(GameSaveData data)
    {   
        OfficeStateMachine.SetState(data.officeState);
    }
}
