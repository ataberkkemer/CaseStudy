using System.Collections.Generic;
using Service.Abstract;
using UnityEngine;

namespace Service.Timer
{
    public class TimerService : MonoService
    {
        private readonly List<GameTimer> _activeTimers = new();

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            for (var i = _activeTimers.Count - 1; i >= 0; i--)
            {
                _activeTimers[i].Update(deltaTime);
                if (_activeTimers[i].IsFinished)
                    _activeTimers.RemoveAt(i);
            }
        }

        public void RegisterTimer(GameTimer timer)
        {
            _activeTimers.Add(timer);
        }

        public void UnregisterTimer(GameTimer timer)
        {
            _activeTimers.Remove(timer);
        }
    }
}