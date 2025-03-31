using System.Collections.Generic;
using Controller.Coin.Events;
using Manager.Score.Events;
using Service;
using Service.Concrete;
using Service.GameStateService.Events;
using Service.GameStateService.Models;
using Service.Timer;
using UnityEngine;

namespace Manager.Score
{
    public class ScoreManager : MonoBehaviour
    {
        private const int WinScore = 20;
        private readonly List<int> _streakMap = new() { 1, 2, 3, 4, 5 };
        private int _collectedCoins;
        private int _currentStreak;
        private GameTimer _gameTimer;
        private int _score;

        private void Start()
        {
            _currentStreak = 0;
            _score = 0;
            ServiceLocator.Get<EventService>().Fire(new UpdateScoreEvent(_score));
        }

        private void OnEnable()
        {
            ServiceLocator.Get<EventService>().Register<CoinCollectedEvent>(OnCoinCollected);
            ServiceLocator.Get<EventService>().Register<GameStateChangedEvent>(OnGameStateChanged);
        }

        private void OnDisable()
        {
            ServiceLocator.Get<EventService>().Unregister<CoinCollectedEvent>(OnCoinCollected);
            ServiceLocator.Get<EventService>().Unregister<GameStateChangedEvent>(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameStateChangedEvent data)
        {
            if (data.GameState != GameState.Playing) return;
            _score = 0;
            _currentStreak = 0;
            _collectedCoins = 0;

            if (_gameTimer != null)
            {
                _gameTimer.Stop();
                _gameTimer.OnTick -= CheckStreak;
                _gameTimer.OnComplete -= CompleteStreak;
                _gameTimer = null;
            }
        }

        private void OnCoinCollected(CoinCollectedEvent data)
        {
            _score += _streakMap[_currentStreak];

            ServiceLocator.Get<EventService>().Fire(new UpdateScoreEvent(_score));

            if (_score >= WinScore)
            {
                ServiceLocator.Get<EventService>().Fire(new GameStateChangedEvent(GameState.Win));
                _score = 0;
                if (_gameTimer != null) CompleteStreak();
                return;
            }

            ServiceLocator.Get<EventService>().Fire(new SpawnCoinEvent(_currentStreak));

            _collectedCoins++;

            if (_gameTimer == null)
            {
                _gameTimer = new GameTimer(3);
                _gameTimer.OnTick += CheckStreak;
                _gameTimer.OnComplete += CompleteStreak;
            }
        }

        private void CompleteStreak()
        {
            _gameTimer.OnTick -= CheckStreak;
            _gameTimer.OnComplete -= CompleteStreak;
            _collectedCoins = 0;
            _gameTimer = null;
        }

        private void CheckStreak(float data)
        {
            if (_collectedCoins >= 3)
            {
                _collectedCoins = 0;
                _currentStreak++;
                _currentStreak = Mathf.Clamp(_currentStreak, 0, _streakMap.Count - 1);
                _gameTimer.Restart();
            }
        }
    }
}