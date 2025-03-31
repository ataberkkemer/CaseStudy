using System;
using UnityEngine;

namespace Service.Timer
{
    public class GameTimer
    {
        private readonly float _duration;
        private float _elapsedTime;
        private bool _isRunning;

        public GameTimer(float seconds)
        {
            _duration = seconds;
            _elapsedTime = 0f;
            _isRunning = true;

            var timerService = ServiceLocator.Get<TimerService>();
            timerService?.RegisterTimer(this);
        }

        public bool IsFinished => _elapsedTime >= _duration;

        public event Action<float> OnTick;
        public event Action OnComplete;

        public void Update(float fixedDeltaTime)
        {
            if (!_isRunning) return;

            _elapsedTime += fixedDeltaTime;
            var remainingTime = Mathf.Max(0, _duration - _elapsedTime);
            OnTick?.Invoke(remainingTime);

            if (IsFinished)
            {
                OnComplete?.Invoke();
                Stop();
            }
        }

        public void Stop()
        {
            _isRunning = false;
            var timerService = ServiceLocator.Get<TimerService>();
            timerService?.UnregisterTimer(this);
        }

        public void Restart()
        {
            _elapsedTime = 0f;
            _isRunning = true;
        }
    }
}