using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Squirrel;

namespace MagpyServerWindows
{
    public class Program
    {
        static public Process child;

        static void Main(string[] args)
        {
            Application.ApplicationExit += Application_ApplicationExit;
            Process.GetCurrentProcess().Exited += Program_Exited;

            NotificationIcon.StartNotificationIcon();

            bool nodeExists = File.Exists("..\\node.exe");

            if (!nodeExists)
            {
                File.Copy(".\\node.exe", "..\\node.exe");
            }

            child = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "..\\node.exe",
                    Arguments = ".\\bundle\\js\\bundle.js",
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

            //checkForUpdates();

            Application.Run();
        }

        private static async void checkForUpdates()
        {
            using (var mgr = new UpdateManager("E:\\Libraries\\Documents\\Projects\\MagpyServerWindows\\Releases"))
            {
                await mgr.UpdateApp();
            }
        }

        private static void Program_Exited(object sender, EventArgs e)
        {
            if (child != null && !child.HasExited)
            {
                child.Kill();
            }
        }

        private static void Child_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private static void Child_Exited(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (child != null && !child.HasExited)
            {
                child.Kill();
            }
        }
    }
}
