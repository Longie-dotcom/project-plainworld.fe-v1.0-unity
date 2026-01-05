using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Service;
using Assets.Service.Interface;
using Assets.Utility;

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
            services[typeof(T)] = service;
        }

        public static T Get<T>() where T : IService
        {
            return (T)services[typeof(T)];
        }

        public static async Task ShutdownAll()
        {
            var playerService = Get<PlayerService>();
            await playerService.LogoutAsync();

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