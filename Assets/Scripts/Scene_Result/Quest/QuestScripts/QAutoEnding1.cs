using UnityEngine;

public class QAutoEnding1 : MonoBehaviour
{
    private void OnEnable()
    {
        if (ResultManager.instance.CurrentStageInfo == 1)
        {
            QuestEventManager.TriggerEvent("ending1");
        }
    }
}
