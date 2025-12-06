using System;
using System.Collections.Generic;

namespace Assets.Core
{
    public static class ServiceLocator
    {
        #region Attributes
        private static readonly Dictionary<Type, IService> services = new();
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static void Register<T>(T service) where T : IService
        {
            var type = typeof(T);

            if (services.ContainsKey(type))
                services[type].Shutdown();

            services[type] = service;
            service.Initialize();
        }

        public static T Get<T>() where T : IService
        {
            return (T)services[typeof(T)];
        }

        public static void ShutdownAll()
        {
            foreach (var service in services.Values)
            {
                service.Shutdown();
            }

            services.Clear();
        }
        #endregion
    }
}