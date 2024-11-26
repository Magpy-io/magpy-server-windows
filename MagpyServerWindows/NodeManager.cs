using Serilog;
using System;
using System.Diagnostics;
using System.IO;
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
                string pathWithPotentialSpaces = RelativeExeToAbsolute(".\\bundle\\js\\bundle.js");

                // Escaping spaces if in path
                return $"\"{pathWithPotentialSpaces}\"";
            }
        }

        public static bool VerifyNodeExe()
        {
            Directory.CreateDirectory(RelativeExeToAbsolute("..\\redis"));

            bool nodeExistsInRedis = File.Exists(RelativeExeToAbsolute("..\\redis\\node.exe"));
            bool nodeExistsInAppFolder = File.Exists(RelativeExeToAbsolute(".\\redis\\node.exe"));

            if (!nodeExistsInRedis && nodeExistsInAppFolder)
            {
                File.Copy(RelativeExeToAbsolute(".\\redis\\node.exe"), RelativeExeToAbsolute("..\\redis\\node.exe"));
                nodeExistsInRedis = true;
            }

            if (nodeExistsInAppFolder)
            {
                File.Delete(RelativeExeToAbsolute(".\\redis\\node.exe"));
            }

            return nodeExistsInRedis;
        }

        public static void StartNodeServer()
        {
            child = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = NodePath,
                    Arguments = JsEntryFilePath,
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

            child.BeginErrorReadLine();
            child.ErrorDataReceived += Child_OutputErrorReceived;
        }

        public static void KillNodeServer()
        {
            if (child != null && !child.HasExited)
            {
                child.Kill();
            }
        }

        public static void SendData(string data)
        {
            if (child != null)
            {
                child.StandardInput.WriteLine(data);
            }
        }

        private static void Child_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            LoggingManager.LoggerNode.Debug(e.Data);
        }

        private static void Child_OutputErrorReceived(object sender, DataReceivedEventArgs e)
        {
            LoggingManager.LoggerNode.Error(e.Data);
        }

        private static void Child_Exited(object sender, EventArgs e)
        {
            Log.Debug("Server node exited. Closing app.");
            Environment.Exit(0);
        }
    }
}
