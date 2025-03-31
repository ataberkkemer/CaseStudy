using Service.Abstract;
using Service.GameStateService.Models;

namespace Service.GameStateService.Events
{
    public class GameStateChangedEvent : IEvent
    {
        public GameStateChangedEvent(GameState gameState)
        {
            GameState = gameState;
        }

        public GameState GameState { get; private set; }
    }
}