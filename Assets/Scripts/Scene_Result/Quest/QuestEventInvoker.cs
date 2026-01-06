using UnityEngine;

public class EventInvoker : MonoBehaviour
{
    public QuestEventId eventId;

    public void InvokeEvent()
    {
        QuestEventManager.TriggerEvent(eventId);
    }
}