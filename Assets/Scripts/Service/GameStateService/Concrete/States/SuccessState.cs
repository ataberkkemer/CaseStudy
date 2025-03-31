using Controller.UI.Models;
using Service.Concrete;
using Service.GameStateService.Abstract;
using Service.GameStateService.Events;

namespace Service.GameStateService.Concrete.States
{
    public class SuccessState : IGameState
    {
        public void Enter()
        {
            ServiceLocator.Get<EventService>().Fire(new OpenPanelEvent(PanelType.Win));
        }

        public void Exit()
        {
            ServiceLocator.Get<EventService>().Fire(new ClosePanelEvent(PanelType.Win));
        }
    }
}