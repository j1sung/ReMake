using System.Collections.Generic;
using UnityEngine;

public class QActionCollectAll : MonoBehaviour
{
    [SerializeField] private int max;
    private bool isTriggered = false;
    [SerializeField] private int count;

    public void CheckCollectAll()
    {
        count = transform.childCount;

        if (!isTriggered && count == max)
        {
            QuestEventManager.TriggerEvent(QuestEventId.collectAll);
            isTriggered = true;
        }
    }
}
