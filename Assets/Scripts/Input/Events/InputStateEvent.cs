using Service.Abstract;

namespace Input.Events
{
    public class InputStateEvent : IEvent
    {
        public InputStateEvent(bool ısInputActive)
        {
            IsInputActive = ısInputActive;
        }

        public bool IsInputActive { get; private set; }
    }
}