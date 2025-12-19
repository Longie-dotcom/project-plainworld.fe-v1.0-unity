using UnityEngine;

/*
 * Centralized logging utility for the game client.
 *
 * Purpose:
 * - Provide a consistent, categorized logging format.
 * - Separate log output by system responsibility (Channel).
 * - Control debug verbosity from a single switch.
 *
 * Design notes:
 * - Wraps UnityEngine.Debug to avoid direct usage across codebase.
 * - Intended for development and debugging only.
 * - Logging can be globally disabled via EnableDebug.
 *
 * Behavior:
 * - Formats logs as: [Level][Channel] Message
 * - Routes messages to Unity's Log / Warning / Error streams.
 *
 * Usage:
 * - GameLogger.Info(Channel.System, "Service initialized");
 * - GameLogger.Warning(Channel.Network, "Connection unstable");
 * - GameLogger.Error(Channel.Gameplay, "Invalid player state");
 */

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