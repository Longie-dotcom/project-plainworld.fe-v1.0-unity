using UnityEngine;

namespace Assets.Utility
{
    public enum Level
    {
        Error,
        Warning,
        Info,
    }

    public enum Channel
    {
        System,
        Network,
        Service,
        Gameplay,
        UI,
    }

    public static class GameLogger
    {
        #region Attributes
        public static bool EnableDebug = true;
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static void Info(Channel channel, string message)
        {
            if (!EnableDebug) return;
            Debug.Log(Format(channel, Level.Info, message));
        }

        public static void Warning(Channel channel, string message)
        {
            if (!EnableDebug) return;
            Debug.LogWarning(Format(channel, Level.Warning, message));
        }

        public static void Error(Channel channel, string message)
        {
            if (!EnableDebug) return;
            Debug.LogError(Format(channel, Level.Error, message));
        }

        private static string Format(Channel channel, Level level, string message)
        {
            return $"[{level}][{channel}] {message}";
        }
        #endregion
    }
}