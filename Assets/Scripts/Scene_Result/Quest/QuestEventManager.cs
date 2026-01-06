using System;
using UnityEngine;

public static class QuestEventManager
{
    public static event Action<QuestEventId> OnEventTriggered;

    public static void TriggerEvent(QuestEventId eventId)
    {
        Debug.Log($"[Event] {eventId}");
        OnEventTriggered?.Invoke(eventId); // 이벤트 송출
    }
}
