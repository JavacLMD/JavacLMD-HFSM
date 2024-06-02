using JavacLMD.EventSystem;
using System;
using UnityEngine;

namespace JavacLMD.EventSystem
{
    public class GlobalEventManager : MonoBehaviour, IGameEventStorage<string>
    {

        private static GlobalEventManager m_Instance;
        public static GlobalEventManager Instance
        {
            get
            {
                Initialize();
                return m_Instance;
            }
        }

        public EventStorage<string> EventStorage { get; private set; } = new EventStorage<string>();


        private static void Initialize()
        {
            if (m_Instance != null) return;

            GameObject gameObject = GameObject.Find("Manager");
            if (gameObject == null) gameObject = new GameObject("Manager");

            DontDestroyOnLoad(gameObject);

            m_Instance = gameObject.GetComponent<GlobalEventManager>();
            if (m_Instance == null) m_Instance = gameObject.AddComponent<GlobalEventManager>();
        }

        public void AddListener(string id, Action listener)
        {
            EventStorage.AddListener(id, listener);
        }

        public void AddListener<TGameEvent>(string id, Action<TGameEvent> listener) where TGameEvent : IGameEvent
        {
            EventStorage.AddListener(id, listener);
        }

        public void RemoveListener(string id, Action listener)
        {
            EventStorage.RemoveListener(id, listener);
        }

        public void RemoveListener<TGameEvent>(string id, Action<TGameEvent> listener) where TGameEvent : IGameEvent
        {
            EventStorage.RemoveListener(id, listener);
        }

        public void TriggerEvent(string eventID)
        {
            EventStorage.TriggerEvent(eventID);
        }

        public void TriggerEvent<TGameEvent>(string eventID, TGameEvent eventData) where TGameEvent : IGameEvent
        {
            EventStorage.TriggerEvent<TGameEvent>(eventID, eventData);
        }
    }


}

