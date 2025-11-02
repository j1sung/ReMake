using System.Collections.Generic;
using UnityEngine;

public class QActionCollectAll : MonoBehaviour
{
    [SerializeField] private List<int> stageMaximum;
    private bool isTriggered = false;
    [SerializeField] private int count;

    public void CheckCollectAll()
    {
        count = transform.childCount;
        int max = stageMaximum[ResultManager.instance.CurrentStageInfo - 1];

        if (!isTriggered && count == max)
        {
            QuestEventManager.TriggerEvent("collectAll");
            isTriggered = true;
        }
    }

    public void ResetTrigger()
    {
        isTriggered = false;
    }
}
