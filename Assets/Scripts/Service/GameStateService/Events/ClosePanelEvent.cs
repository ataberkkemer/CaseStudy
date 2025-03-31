using Controller.UI.Models;
using Service.Abstract;

namespace Service.GameStateService.Events
{
    public class ClosePanelEvent : IEvent
    {
        public ClosePanelEvent(PanelType panelType)
        {
            PanelType = panelType;
        }

        public PanelType PanelType { get; private set; }
    }
}