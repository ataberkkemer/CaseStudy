using System;
using System.Linq;
using UnityEngine;

namespace Factory.Pool.Model
{
    [CreateAssetMenu(fileName = "Pool", menuName = "Object Pooling/Create Pool", order = 0)]
    public class PoolDataSo : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("ID of the pool")] public int id;

        public int defaultCapacity = 1;

        public int maxSize = 1;

        public PoolType poolType;

        public GameObject prefab;

        public string plainCSharpTypeName;

        public bool activateOnGet = true;

        public string pooledObjectTypeName;

        public Type PlainCSharpType;

        public Type PooledObjectType;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            PooledObjectType = FindType(pooledObjectTypeName);

            PlainCSharpType = FindType(plainCSharpTypeName);
        }

        private Type FindType(string typeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(assembly => assembly.GetTypes().Any(type => type.FullName == typeName))
                ?.GetType(typeName);
        }
    }
}