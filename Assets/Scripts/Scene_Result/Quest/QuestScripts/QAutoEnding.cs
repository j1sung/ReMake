using Unity.VisualScripting;
using UnityEngine;

public class QAutoEnding : MonoBehaviour
{   
    private void OnEnable()
    {   
        if (ResultManager.instance.CurrentStageInfo == 1 
            && OfficeStateMachine.currentState != OfficeState.ReadyStage2
            && OfficeStateMachine.currentState != OfficeState.ReadyStage3)
        {   
            QuestEventManager.TriggerEvent(QuestEventId.ending1);
        }
        else if (ResultManager.instance.CurrentStageInfo == 2 
            && OfficeStateMachine.currentState != OfficeState.ReadyStage3)
        {
            //QuestEventManager.TriggerEvent(QuestEventId.ending2);
        }       
    }
}
