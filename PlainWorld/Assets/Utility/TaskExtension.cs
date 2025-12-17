using System.Collections;
using System.Threading.Tasks;

/*
 * Unity coroutine bridge for async Tasks.
 *
 * Purpose:
 * - Allow awaiting Tasks inside Unity coroutines.
 * - Used mainly for service initialization / shutdown.
 *
 * Behavior:
 * - Yields until task completion.
 * - Rethrows exceptions to Unity coroutine system.
 */

namespace Assets.Utility
{
    public static class TaskExtensions
    {
        public static IEnumerator AsCoroutine(this Task task)
        {
            while (!task.IsCompleted)
                yield return null;

            if (task.IsFaulted)
                throw task.Exception;
        }
    }
}
