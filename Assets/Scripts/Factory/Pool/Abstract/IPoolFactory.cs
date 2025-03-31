using Factory.Pool.Model;
using UnityEngine;
using UnityEngine.Pool;

namespace Factory.Pool.Abstract
{
    public interface IPoolFactory
    {
        ObjectPool<T> CreatePool<T>(PoolDataSo data, Transform parent) where T : class;
    }
}