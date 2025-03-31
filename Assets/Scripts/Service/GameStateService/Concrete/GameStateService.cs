using Service.Abstract;
using Service.Concrete;
using Service.GameStateService.Events;
using Service.GameStateService.Models;

namespace Service.GameStateService.Concrete
{
    public class GameStateService : MonoService
    {
        private GameStateMachine _gameStateMachine;
        public GameState GameState { get; private set; }

        private void Start()
        {
            ServiceLocator.Get<EventService>().Register<GameStateChangedEvent>(OnGameStateChanged);
            _gameStateMachine = new GameStateMachine();
        }

        private void OnDisable()
        {
            ServiceLocator.Get<EventService>().Register<GameStateChangedEvent>(OnGameStateChanged);
            _gameStateMachine.Dispose();
        }

        private void OnGameStateChanged(GameStateChangedEvent data)
        {
            _gameStateMachine.SetState(data.GameState);
        }
    }
}