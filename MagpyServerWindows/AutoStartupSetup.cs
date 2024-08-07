using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagpyServerWindows
{
    public class AutoStartupSetup
    {
        const string START_UP_REGKEY_NAME = "Magpy";
        public static void EnableAutoStartup()
        {
            RegistryKey keyRun = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            keyRun.SetValue(START_UP_REGKEY_NAME, "\"" + Application.ExecutablePath + "\"");
        }

        public static void DisableAutoStartup()
        {
            RegistryKey keyRun = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            keyRun.DeleteValue(START_UP_REGKEY_NAME);
        }
    }
}
