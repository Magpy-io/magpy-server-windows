using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagpyServerWindows
{
    class PathManager
    {
        public static string RelativeExeToAbsolute(string relativePath)
        {
            return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), relativePath);
        }
    }
}
