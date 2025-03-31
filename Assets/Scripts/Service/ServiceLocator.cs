using System;
using System.Collections.Generic;
using Service.Abstract;
using UnityEngine;

namespace Service
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, IService> services = new();

        public static void Register<T>(T service) where T : IService
        {
            Register(service, typeof(T));
        }

        public static void Register(IService service, Type type)
        {
            if (services.ContainsKey(type))
            {
                Debug.LogWarning($"Service {type} is already registered.");
                return;
            }

            services[type] = service;
        }

        public static T Get<T>() where T : class, IService
        {
            services.TryGetValue(typeof(T), out var service);
            return service as T;
        }

        public static void Unregister<T>() where T : class, IService
        {
            services.Remove(typeof(T));
        }

        public static void Unregister(Type type)
        {
            services.Remove(type);
        }
    }
}