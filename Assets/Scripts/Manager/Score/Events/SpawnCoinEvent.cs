using Service.Abstract;

namespace Manager.Score.Events
{
    public class SpawnCoinEvent : IEvent
    {
        public SpawnCoinEvent(int currentStreak)
        {
            ID = currentStreak;
        }

        public int ID { get; private set; }
    }
}