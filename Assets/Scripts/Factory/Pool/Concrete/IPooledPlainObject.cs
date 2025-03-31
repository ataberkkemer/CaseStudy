using Factory.Pool.Abstract;

namespace Factory.Pool.Concrete
{
    public interface IPooledPlainObject : IPooledObject
    {
        void OnGet();

        void ManualRelease();
    }
}