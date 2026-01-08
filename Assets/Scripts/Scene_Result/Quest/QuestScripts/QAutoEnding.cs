using Unity.VisualScripting;
using UnityEngine;

public class QAutoEnding : MonoBehaviour
{
    private void OnEnable()
    {
        if (ResultManager.instance.CurrentStageInfo == 1)
        {
            QuestEventManager.TriggerEvent(QuestEventId.ending1);
            ResultManager.instance.SetNextStage();
            OfficeStateMachine.SetState(OfficeState.Stage1Clear);
        }
        else if (ResultManager.instance.CurrentStageInfo == 2)
        {
            // QuestEventManager.TriggerEvent(QuestEventId.ending2);
            //ResultManager.instance.SetNextStage();
            // OfficeStateMachine.SetState(OfficeState.Stage2Clear);
        }
    }
}
