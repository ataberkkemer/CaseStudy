using System;
using System.Collections.Generic;
using Factory.Pool.Abstract;
using Factory.Pool.Concrete;
using Factory.Pool.Model;
using Service.Abstract;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Service.Concrete
{
    public class PoolService : MonoService
    {
        private const string PoolParent = "PoolParent";
        private IPoolFactory _poolFactory;
        private Transform _poolParent;

        private Dictionary<int, object> Pool { get; } = new();

        protected override void Awake()
        {
            base.Awake();

            _poolFactory = new PoolFactory();
            _poolParent = new GameObject(PoolParent).transform;
        }

        public void Release<T>(int id, T obj, Action onRelease) where T : class
        {
            if (Pool.TryGetValue(id, out var value) && value is ObjectPool<object> pool)
            {
                pool.Release(obj);
                onRelease?.Invoke();
            }
            else
            {
                Debug.LogError($"Pool for ID {id} does not exist");
            }
        }

        public T Get<T>(PoolDataSo data) where T : class
        {
            if (!Pool.ContainsKey(data.id)) AddToPool(data);

            if (Pool[data.id] is not ObjectPool<object> pool)
            {
                Debug.LogError($"Pool for ID {data.id} does not match type {typeof(T)}");
                return null;
            }

            var result = pool.Get();
            var targetResult = data.poolType switch
            {
                PoolType.PlainCSharp => result as T,
                PoolType.GameObject => result as GameObject as T,
                _ => (result as GameObject)?.GetComponent<T>()
            };
            if (targetResult != null) return targetResult;
            Release(data.id, result, () => Debug.LogError($"Object type does not match the target type {typeof(T)}"));
            return null;
        }

        public T Get<T>(PoolDataSo data, Vector3 position, Quaternion rotation) where T : Object
        {
            var obj = Get<T>(data);
            switch (data.poolType)
            {
                case PoolType.GameObject:
                    (obj as GameObject)!.GetComponent<IPooledGameObject>()
                        .SetTransform(position, rotation, _poolParent);
                    break;
                case PoolType.Component:
                    (obj as Component)!.GetComponent<IPooledGameObject>().SetTransform(position, rotation, _poolParent);
                    break;
            }

            return obj;
        }

        public T Get<T>(PoolDataSo data, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            var obj = Get<T>(data);
            switch (data.poolType)
            {
                case PoolType.GameObject:
                    (obj as GameObject)!.GetComponent<IPooledGameObject>().SetTransform(position, rotation, parent);
                    break;
                case PoolType.Component:
                    (obj as Component)!.GetComponent<IPooledGameObject>().SetTransform(position, rotation, parent);
                    break;
            }

            return obj;
        }

        private void AddToPool(PoolDataSo data)
        {
            var result = _poolFactory.CreatePool<object>(data, _poolParent);
            Pool.Add(data.id, result);
        }
    }
}