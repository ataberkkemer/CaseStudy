using Service.Abstract;

namespace Manager.Score.Events
{
    public class UpdateScoreEvent : IEvent
    {
        public UpdateScoreEvent(int score)
        {
            Score = score;
        }

        public int Score { get; private set; }
    }
}