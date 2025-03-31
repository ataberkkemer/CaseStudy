using Factory.Pool.Abstract;
using UnityEngine;

namespace Factory.Pool.Concrete
{
    public interface IPooledGameObject : IPooledObject
    {
        public Transform Parent { get; set; }

        void OnGet(bool activateOnGet);

        void SetTransform(Vector3 position, Quaternion rotation, Transform parent);

        void ManualRelease(bool releaseChildren = true);
    }
}