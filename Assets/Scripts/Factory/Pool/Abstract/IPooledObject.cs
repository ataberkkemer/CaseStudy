namespace Factory.Pool.Abstract
{
    public interface IPooledObject
    {
        public int Id { get; set; }

        void OnRelease();
    }
}