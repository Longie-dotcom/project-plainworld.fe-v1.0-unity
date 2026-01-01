using System;
using System.Threading.Tasks;
using Assets.Core;

/*
 * Presenter / Application layer helper.
 *
 * Purpose:
 * - Safely run fire-and-forget async workflows from Unity callbacks.
 * - Catch both sync and async exceptions. (MAIN REASON)
 * - Report errors on the Unity main thread. (MAIN REASON)
 *
 * NOT intended for:
 * - Service layer
 * - Domain logic
 * - State mutation
 */

namespace Assets.Utility
{
    public static class AsyncHelper
    {
        public static void Run(Func<Task> taskFactory)
        {
            Task task;

            try
            {
                task = taskFactory();
            }
            catch (Exception ex)
            {
                Report(ex);
                return;
            }

            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    Report(t.Exception);
            }, TaskScheduler.Default);
        }

        private static void Report(Exception ex)
        {
            CoroutineRunner.Instance.Schedule(() =>
            {
                GameLogger.Error(
                    Channel.Gameplay, 
                    $"There is an exception: {ex.Message}");
            });
        }
    }
}
