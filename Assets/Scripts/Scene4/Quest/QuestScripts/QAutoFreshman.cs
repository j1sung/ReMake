using UnityEngine;

public class QAutoFreshman : MonoBehaviour
{   
    private void OnEnable()
    {  
        if(ResultManager.instance.CurrentStageInfo == 1)
        {
            QuestEventManager.TriggerEvent("freshman");
            enabled = false;
        }
    }
}
