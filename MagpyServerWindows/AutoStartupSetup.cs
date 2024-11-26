using Microsoft.Win32;
using System.Reflection;

namespace MagpyServerWindows
{
    public class AutoStartupSetup
    {
        const string START_UP_REGKEY_NAME = "Magpy";
        const string START_UP_REGKEY_PATH = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        public static void EnableAutoStartup()
        {
            RegistryKey keyRun = Registry.CurrentUser.OpenSubKey(START_UP_REGKEY_PATH, true);
            keyRun.SetValue(START_UP_REGKEY_NAME, Assembly.GetExecutingAssembly().Location);
        }

        public static void DisableAutoStartup()
        {
            RegistryKey keyRun = Registry.CurrentUser.OpenSubKey(START_UP_REGKEY_PATH, true);
            keyRun.DeleteValue(START_UP_REGKEY_NAME);
        }
    }
}
