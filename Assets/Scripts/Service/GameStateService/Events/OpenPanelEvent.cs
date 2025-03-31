using Controller.UI.Models;
using Service.Abstract;

namespace Service.GameStateService.Events
{
    public class OpenPanelEvent : IEvent
    {
        public OpenPanelEvent(PanelType panelType)
        {
            PanelType = panelType;
        }

        public PanelType PanelType { get; private set; }
    }
}