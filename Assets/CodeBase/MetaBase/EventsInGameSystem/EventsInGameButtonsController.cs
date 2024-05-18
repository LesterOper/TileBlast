using System;
using UnityEngine;
using Zenject;

namespace CodeBase.MetaBase.EventsInGameSystem
{
    public class EventsInGameButtonsController : MonoBehaviour
    {
        private EventsInGameConfig _eventsInGameConfig;

        [Inject]
        private void Construct(EventsInGameConfig eventsInGameConfig)
        {
            _eventsInGameConfig = eventsInGameConfig;
        }

        private void Start()
        {
            var events = _eventsInGameConfig.GetActiveEvents();

            for (int i = 0; i < events.Count; i++)
            {
                EventButton eventButton = Instantiate(events[i].buttonEventPrefab, transform);
                eventButton.Initialize(events[i]);
            }
        }
    }
}