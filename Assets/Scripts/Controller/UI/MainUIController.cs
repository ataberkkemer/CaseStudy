using Manager.Score.Events;
using Service;
using Service.Concrete;
using Service.GameStateService.Events;
using Service.GameStateService.Models;
using TMPro;
using UnityEngine;

namespace Controller.UI
{
    public class MainUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private void OnEnable()
        {
            ServiceLocator.Get<EventService>().Register<GameStateChangedEvent>(OnGameStateChanged);
            ServiceLocator.Get<EventService>().Register<UpdateScoreEvent>(OnUpdateScore);
        }

        private void OnDisable()
        {
            ServiceLocator.Get<EventService>().Unregister<GameStateChangedEvent>(OnGameStateChanged);
            ServiceLocator.Get<EventService>().Unregister<UpdateScoreEvent>(OnUpdateScore);
        }

        private void OnUpdateScore(UpdateScoreEvent data)
        {
            scoreText.SetText(data.Score.ToString());
        }

        private void OnGameStateChanged(GameStateChangedEvent data)
        {
            if (data.GameState == GameState.Starting) scoreText.SetText("0");
        }
    }
}