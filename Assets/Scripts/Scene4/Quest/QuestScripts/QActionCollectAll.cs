using System.Collections.Generic;
using UnityEngine;

public class QActionCollectAll : MonoBehaviour
{
    [SerializeField] private List<int> stageMaximum;
    [SerializeField] private int count; // µð¹ö±ë¿ë
    private void Update()
    {
        count = transform.childCount;
        if (count == stageMaximum[ResultManager.instance.CurrentStageInfo - 1])
            QuestEventManager.TriggerEvent("collectAll");
    }
}
