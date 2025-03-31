using System;
using Factory.Pool.Abstract;
using Factory.Pool.Model;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Factory.Pool.Concrete
{
    public class PoolFactory : IPoolFactory
    {
        public ObjectPool<T> CreatePool<T>(PoolDataSo data, Transform parent) where T : class
        {
            return new ObjectPool<T>(
                () => Create<T>(data, parent),
                poolObject => SetOnGetAction(poolObject, data),
                poolObject =>
                {
                    switch (data.poolType)
                    {
                        case PoolType.GameObject:
                        case PoolType.Component:
                        {
                            var obj = poolObject as GameObject;
                            if (obj != null) obj.GetComponent<IPooledGameObject>().OnRelease();
                            break;
                        }
                        default:
                            (poolObject as IPooledObject)?.OnRelease();
                            break;
                    }
                },
                poolObject =>
                {
                    if (data.poolType != PoolType.PlainCSharp)
                        Object.Destroy(poolObject as GameObject);
                },
                false, data.defaultCapacity, data.maxSize);
        }

        private T Create<T>(PoolDataSo data, Transform parent) where T : class
        {
            if (data.poolType != PoolType.PlainCSharp)
            {
                var prefab = data.prefab;
                var obj = Object.Instantiate(prefab, parent);

                var type = obj.AddComponent<PooledObject>();

                var poolObject = (IPooledGameObject)type;
                poolObject.Id = data.id;
                poolObject.Parent = parent;

                return obj as T;
            }
            else
            {
                var obj = Activator.CreateInstance(data.PlainCSharpType);
                var poolObject = (IPooledPlainObject)obj;
                poolObject.Id = data.id;
                return obj as T;
            }
        }

        private void SetOnGetAction<T>(T obj, PoolDataSo data) where T : class
        {
            switch (data.poolType)
            {
                case PoolType.GameObject:
                case PoolType.Component:
                    (obj as GameObject)?.GetComponent<IPooledGameObject>().OnGet(data.activateOnGet);
                    break;
                case PoolType.PlainCSharp:
                    ((IPooledPlainObject)obj).OnGet();
                    break;
            }
        }
    }
}