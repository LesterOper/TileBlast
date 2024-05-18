using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.MetaBase.EventsInGameSystem
{
    [CreateAssetMenu(menuName = "MetaConfigs/" + nameof(EventsInGameConfig), fileName = nameof(EventsInGameConfig))]
    public class EventsInGameConfig : ScriptableObject
    {
        public List<EventInGame> events;

        public List<EventInGame> GetActiveEvents() => events.FindAll(local => local.activate);
    }

    [Serializable]
    public class EventInGame
    {
        public EventType eventType;
        public int duration;
        public EventButton buttonEventPrefab;

        public bool activate;
    }

    public enum EventType
    {
        None,
        Event1,
        Event2,
        Event3,
    }
}