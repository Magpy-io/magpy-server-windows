using System.Threading;

namespace MagpyServerWindows
{
    class InstanceManager
    {
        private const string SINGLE_APP_INSTANCE_MUTEX_ID = "Magpy-mutex-one-instance_1ec5de2a-0872-4620-b4fc-a0a739e333ac";
        private static Mutex mutex;
        public static bool HoldInstance()
        {
            mutex = new Mutex(false, SINGLE_APP_INSTANCE_MUTEX_ID);
            return mutex.WaitOne(0, false);
        }

        public static void ReleaseInstance()
        {
            if (mutex != null)
            {
                mutex.Close();
                mutex = null;
            }
        }
    }
}
