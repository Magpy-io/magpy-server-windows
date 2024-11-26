using System.Diagnostics;

namespace MagpyServerWindows
{
    public class ServerManager
    {
        public static void OpenWebInterface()
        {
            Process.Start(Constants.serverUrl);
        }
    }
}
