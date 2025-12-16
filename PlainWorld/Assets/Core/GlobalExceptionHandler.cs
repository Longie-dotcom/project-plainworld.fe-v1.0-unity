using Assets.Utility;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core
{
    public static class GlobalExceptionHandler
    {
        #region Attributes
        private static bool initialized = false;
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static void Initialize()
        {
            if (initialized) return;
            initialized = true;

            // Catch Unity main thread exceptions
            Application.logMessageReceived += HandleUnityLog;

            // Catch Task-based async exceptions
            TaskScheduler.UnobservedTaskException += HandleUnobservedTaskException;

            GameLogger.Info(
                Channel.System, 
                "[GlobalExceptionHandler] Initialized.");
        }

        private static void HandleUnityLog(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Exception)
            {
                GameLogger.Error(
                    Channel.System, 
                    "[Unity Exception] " + condition + "\n" + stackTrace);
            }
        }

        private static void HandleUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            GameLogger.Error(
                Channel.System, 
                "[Unobserved Task Exception] " + e.Exception);
            e.SetObserved();
        }

        public static void Shutdown()
        {
            Application.logMessageReceived -= HandleUnityLog;
            TaskScheduler.UnobservedTaskException -= HandleUnobservedTaskException;

            GameLogger.Info(
                Channel.System, 
                "[GlobalExceptionHandler] Shutdown.");
        }
        #endregion
    }
}
