using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static MagpyServerWindows.PathManager;

namespace MagpyServerWindows
{
    class NodeExeManager
    {

        public static string NodePath {
            get {
                return RelativeExeToAbsolute("..\\redis\\node.exe");
            } 
        }

        public static string JsEntryFilePath
        {
            get{
                return RelativeExeToAbsolute(".\\bundle\\js\\bundle.js");
            }
        }

        public static bool VerifyNodeExe()
        {
            Directory.CreateDirectory(RelativeExeToAbsolute("..\\redis"));

            bool nodeExistsInRedis = File.Exists(RelativeExeToAbsolute("..\\redis\\node.exe"));
            bool nodeExistsInAppFolder = File.Exists(RelativeExeToAbsolute(".\\node.exe"));

            if (!nodeExistsInRedis && nodeExistsInAppFolder)
            {
                File.Copy(RelativeExeToAbsolute(".\\node.exe"), RelativeExeToAbsolute("..\\redis\\node.exe"));
                nodeExistsInRedis = true;
            }

            if (nodeExistsInAppFolder)
            {
                File.Delete(RelativeExeToAbsolute(".\\node.exe"));
            }

            return nodeExistsInRedis;
        }
    }
}
