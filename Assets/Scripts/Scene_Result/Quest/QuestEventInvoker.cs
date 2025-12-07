using UnityEngine;

public class EventInvoker : MonoBehaviour
{
    public string eventName;

    public void InvokeEvent()
    {
        QuestEventManager.TriggerEvent(eventName);
    }
}