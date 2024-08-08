using System;
using System.IO;
using System.Windows.Forms;

namespace MagpyServerWindows
{
    class PathManager
    {
        public const string APPDATA_MAGPY_FOLDER_NAME = "Magpy";
        public static string RelativeExeToAbsolute(string relativePath)
        {
            return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), relativePath);
        }

        public static string RelativeAppDataToAbsolute(string relativePath)
        {
            var a = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPDATA_MAGPY_FOLDER_NAME, relativePath);
            return a;
        }

        public static void ClearAppDataFolder()
        {
            Directory.Delete(RelativeAppDataToAbsolute("."), true);
        }
    }
}
