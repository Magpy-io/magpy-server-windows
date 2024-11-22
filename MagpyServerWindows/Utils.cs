using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagpyServerWindows
{
    public class Utils
    {
        public static void SchedulePeriodicTask(Func<Task> action, int seconds, CancellationToken token = default)
        {
            if (action == null)
                return;
            Task.Run(async () => {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(seconds), token);
                    await action();
                }
            }, token);
        }

        public static void ScheduleTask(Func<Task> action, int seconds, CancellationToken token = default)
        {
            if (action == null)
                return;
            Task.Run(async () => {
                if (!token.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(seconds), token);
                    await action();
                }
            }, token);
        }
    }
}
