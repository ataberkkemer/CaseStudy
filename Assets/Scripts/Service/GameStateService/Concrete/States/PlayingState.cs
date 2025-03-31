using Input.Events;
using Service.Concrete;
using Service.GameStateService.Abstract;

namespace Service.GameStateService.Concrete.States
{
    public class PlayingState : IGameState
    {
        public void Enter()
        {
            ServiceLocator.Get<EventService>().Fire(new InputStateEvent(true));
        }

        public void Exit()
        {
            ServiceLocator.Get<EventService>().Fire(new InputStateEvent(false));
        }
    }
}