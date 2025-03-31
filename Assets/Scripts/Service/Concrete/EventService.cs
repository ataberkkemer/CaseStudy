using System;
using System.Collections.Generic;
using Service.Abstract;

namespace Service.Concrete
{
    public class EventService : MonoService
    {
        private readonly Dictionary<Type, List<Delegate>> eventListeners = new();
        private readonly Queue<IEvent> eventQueue = new();

        private void Update()
        {
            while (eventQueue.Count > 0)
            {
                var evt = eventQueue.Dequeue();
                Fire(evt);
            }
        }


        public void Register<T>(Action<T> callback) where T : IEvent
        {
            var eventType = typeof(T);
            if (!eventListeners.ContainsKey(eventType)) eventListeners[eventType] = new List<Delegate>();
            eventListeners[eventType].Add(callback);
        }


        public void Unregister<T>(Action<T> callback) where T : IEvent
        {
            var eventType = typeof(T);
            if (eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType].Remove(callback);
                if (eventListeners[eventType].Count == 0) eventListeners.Remove(eventType);
            }
        }

        public void Fire<T>(T evt) where T : IEvent
        {
            var eventType = typeof(T);
            if (eventListeners.ContainsKey(eventType))
            {
                var listenersCopy = new List<Delegate>(eventListeners[eventType]);

                foreach (var listener in listenersCopy) (listener as Action<T>)?.Invoke(evt);
            }
        }

        public void QueueEvent(IEvent evt)
        {
            eventQueue.Enqueue(evt);
        }
    }
}