using System;
using System.Collections.Generic;
using GameApplication.Gameplay.Models;
using UnityEngine;

namespace GameApplication.Gameplay.Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

        private List<TurnEvent> _eventSequence;
        private int _currentEventIndex = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void LoadEvents(TextAsset jsonFile)
        {
            if (jsonFile == null)
            {
                Debug.LogError("Events JSON file is null!");
                _eventSequence = new List<TurnEvent>();
                return;
            }

            try
            {
                EventSequence sequence = JsonUtility.FromJson<EventSequence>(jsonFile.text);
                _eventSequence = sequence.events ?? new List<TurnEvent>();
                _currentEventIndex = 0;
                Debug.Log($"Загружено событий: {_eventSequence.Count}");
                
                if (_eventSequence.Count > 0)
                {
                    Debug.Log($"Первое событие: {_eventSequence[0].title}");
                    if (_eventSequence[0].resourceChanges != null)
                    {
                        Debug.Log($"  Изменения: W:{_eventSequence[0].resourceChanges.weapons} S:{_eventSequence[0].resourceChanges.supplies} P:{_eventSequence[0].resourceChanges.people}");
                    }
                    else
                    {
                        Debug.LogWarning("  resourceChanges = null!");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка загрузки событий: {e.Message}\n{e.StackTrace}");
                _eventSequence = new List<TurnEvent>();
            }
        }

        public TurnEvent GetCurrentEvent()
        {
            if (_eventSequence == null || _eventSequence.Count == 0)
                return null;

            if (_currentEventIndex >= _eventSequence.Count)
            {
                _currentEventIndex = 0;
            }

            return _eventSequence[_currentEventIndex];
        }

        public TurnEvent GetNextEvent()
        {
            if (_eventSequence == null || _eventSequence.Count == 0)
                return null;

            if (_currentEventIndex >= _eventSequence.Count)
            {
                _currentEventIndex = 0;
            }

            TurnEvent nextEvent = _eventSequence[_currentEventIndex];
            _currentEventIndex++;
            
            return nextEvent;
        }

        public TurnEvent PeekNextEvent()
        {
            if (_eventSequence == null || _eventSequence.Count == 0)
                return null;

            int peekIndex = _currentEventIndex;
            if (peekIndex >= _eventSequence.Count)
            {
                peekIndex = 0;
            }

            return _eventSequence[peekIndex];
        }

        public void Reset()
        {
            _currentEventIndex = 0;
        }

        public int GetTotalEvents()
        {
            return _eventSequence?.Count ?? 0;
        }
    }
}
