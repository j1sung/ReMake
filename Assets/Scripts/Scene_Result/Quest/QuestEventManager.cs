using System;
using UnityEngine;

public static class QuestEventManager
{
    public static Action<string> OnEventTriggered;

    public static void TriggerEvent(string eventName)
    {
        Debug.Log($"[Event] {eventName}");
        OnEventTriggered?.Invoke( eventName ); // 이벤트 송출
    }
}
