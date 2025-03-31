using UnityEngine;

namespace Service.Abstract
{
    [DefaultExecutionOrder(-9999)]
    public abstract class MonoService : MonoBehaviour, IService
    {
        protected virtual void Awake()
        {
            Register();
        }

        public void Register()
        {
            ServiceLocator.Register(this, GetType());
        }
    }
}