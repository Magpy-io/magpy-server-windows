using System.Reflection;

namespace MagpyServerWindows
{
    public class Constants
    {
        public const string serverUrl = "http:\\127.0.0.1:31803";
        public static string AppName = Assembly.GetExecutingAssembly().GetName().Name;
        public static string version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
    }
}
