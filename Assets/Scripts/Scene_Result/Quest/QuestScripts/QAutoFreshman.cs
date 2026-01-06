using UnityEngine;

public class QAutoFreshman : MonoBehaviour
{   
    private void OnEnable()
    {  
        if(ResultManager.instance.CurrentStageInfo == 1)
        {
            QuestEventManager.TriggerEvent(QuestEventId.freshman);
            enabled = false;
        }
    }
}
