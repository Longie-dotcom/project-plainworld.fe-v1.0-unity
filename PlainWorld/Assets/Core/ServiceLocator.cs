using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assets.Core
{
    public static class ServiceLocator
    {
        #region Attributes
        private static readonly Dictionary<Type, IService> services = new();
        #endregion

        #region Properties
        public static event Func<Task> OnExiting;
        #endregion

        #region Methods
        public static void Register<T>(T service) where T : IService
        {
            services[typeof(T)] = service;
        }

        public static T Get<T>() where T : IService
        {
            return (T)services[typeof(T)];
        }

        public static async Task ShutdownAll()
        {
            if (OnExiting != null)
            {
                foreach (Func<Task> handler in OnExiting.GetInvocationList())
                {
                    await handler();
                }
            }

            foreach (var service in services.Values)
            {
                await service.ShutdownAsync();
            }

            services.Clear();
        }

        public static bool IsRegistered<T>() where T : IService
        {
            return services.ContainsKey(typeof(T));
        }
        #endregion
    }
}