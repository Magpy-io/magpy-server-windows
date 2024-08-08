using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static MagpyServerWindows.PathManager;

namespace MagpyServerWindows
{
    class NodeManager
    {
        static public Process child;

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

        public static void StartNodeServer()
        {
            child = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = NodeManager.NodePath,
                    Arguments = NodeManager.JsEntryFilePath,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };
            child.Start();
            ChildProcessTracker.AddProcess(child);
            child.EnableRaisingEvents = true;
            child.Exited += Child_Exited;

            child.BeginOutputReadLine();
            child.OutputDataReceived += Child_OutputDataReceived;
            child.ErrorDataReceived += Child_OutputDataReceived;
        }

        public static void KillNodeServer()
        {
            if (child != null && !child.HasExited)
            {
                child.Kill();
            }
        }

        private static void Child_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            LoggingManager.LoggerNode.Debug(e.Data);
        }

        private static void Child_Exited(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
