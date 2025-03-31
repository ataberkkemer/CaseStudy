using Service;
using Service.Concrete;
using Service.GameStateService.Events;
using Service.GameStateService.Models;
using Service.Timer;
using TMPro;
using UnityEngine;

namespace Controller.UI
{
    public class UITimerController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;

        private GameTimer _gameTimer;

        private void OnEnable()
        {
            ServiceLocator.Get<EventService>().Register<GameStateChangedEvent>(OnGameStateChanged);
        }

        private void OnDisable()
        {
            ServiceLocator.Get<EventService>().Unregister<GameStateChangedEvent>(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameStateChangedEvent data)
        {
            if (data.GameState == GameState.Win)
            {
                if (_gameTimer != null)
                {
                    _gameTimer.Stop();
                    _gameTimer.OnTick -= UpdateTimer;
                    _gameTimer.OnComplete -= CompleteTimer;
                    _gameTimer = null;
                }
            }
            else if (data.GameState == GameState.Playing && _gameTimer == null)
            {
                _gameTimer = new GameTimer(180);
                _gameTimer.OnTick += UpdateTimer;
                _gameTimer.OnComplete += CompleteTimer;
            }
        }

        private void CompleteTimer()
        {
            _gameTimer.OnTick -= UpdateTimer;
            _gameTimer.OnComplete -= CompleteTimer;
            _gameTimer = null;
            ServiceLocator.Get<EventService>().Fire(new GameStateChangedEvent(GameState.Fail));
        }

        private void UpdateTimer(float time)
        {
            var minutes = Mathf.FloorToInt(time / 60);
            var seconds = Mathf.FloorToInt(time % 60);

            timerText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }
}