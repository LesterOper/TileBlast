using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MetaBase.EventsInGameSystem
{
    public class EventButton : MonoBehaviour
    {
        [SerializeField] private Button actionButton;
        [SerializeField] private TextMeshProUGUI timer;

        private EventInGame _eventInGame;

        private void Start()
        {
            actionButton.onClick.AddListener(() =>
            {
                Debug.Log(_eventInGame.eventType);
            });
        }

        public void Initialize(EventInGame eventInGame)
        {
            _eventInGame = eventInGame;
            timer.text = eventInGame.duration.ToString();
        }
    }
}