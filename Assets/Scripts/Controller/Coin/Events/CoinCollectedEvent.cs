using Service.Abstract;

namespace Controller.Coin.Events
{
    public class CoinCollectedEvent : IEvent
    {
        public CoinCollectedEvent(int id)
        {
            ID = id;
        }

        public int ID { get; private set; }
    }
}