using System;
using System.Collections.Generic;

namespace CodingTest.Utility
{
    public static class ServiceContainer
    {
        private static readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        public static void AddInstance<T>(T instance)
        {
            var type = typeof(T);
            if(!_instances.ContainsKey(type)) _instances.Add(type, instance);
        }

        public static T GetInstance<T>()
        {
            if (_instances.ContainsKey(typeof(T))) return (T)_instances[typeof(T)];
            
            return default;
        }

        public static void RemoveInstance<T>(T instance)
        {
            if (_instances.ContainsKey(typeof(T))) _instances.Remove(typeof(T));
        }
    }
}