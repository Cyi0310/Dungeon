using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;

public abstract class BaseEventTrigger : MonoBehaviour
{
    [SerializeField] protected EventTrigger eventTrigger;
    protected readonly Dictionary<EventTriggerType, UnityAction<BaseEventData>> eventCallbackMap = new();

    protected void RegisterEvent(EventTriggerType eventType, UnityAction<BaseEventData> callback)
    {
        if (eventCallbackMap.ContainsKey(eventType))
        {
            Debug.LogWarning($"Event {eventType} is already registered.");
            return;
        }

        var entry = new EventTrigger.Entry
        {
            eventID = eventType
        };
        entry.callback.AddListener(callback);
        eventTrigger.triggers.Add(entry);

        eventCallbackMap[eventType] = callback;
    }

    protected void UnregisterEvent(EventTriggerType eventType)
    {
        // 檢查是否已經註冊
        if (!eventCallbackMap.ContainsKey(eventType))
        {
            Debug.LogWarning($"Event {eventType} is not registered.");
            return;
        }

        var entryToRemove = eventTrigger.triggers
                    .FirstOrDefault(entry => entry.eventID == eventType && entry.callback.GetPersistentEventCount() > 0);

        if (entryToRemove != null)
        {
            eventTrigger.triggers.Remove(entryToRemove);
        }

        eventCallbackMap.Remove(eventType);
    }
}
