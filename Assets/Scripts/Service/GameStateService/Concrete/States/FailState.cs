using Controller.UI.Models;
using Service.Concrete;
using Service.GameStateService.Abstract;
using Service.GameStateService.Events;

namespace Service.GameStateService.Concrete.States
{
    public class FailState : IGameState
    {
        public void Enter()
        {
            ServiceLocator.Get<EventService>().Fire(new OpenPanelEvent(PanelType.Lose));
        }

        public void Exit()
        {
            ServiceLocator.Get<EventService>().Fire(new ClosePanelEvent(PanelType.Lose));
        }
    }
}