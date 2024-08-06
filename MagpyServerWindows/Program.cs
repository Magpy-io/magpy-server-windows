using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Velopack;

namespace MagpyServerWindows
{
    public class Program
    {
        static public Process child;

        static void Main(string[] args)
        {
            VelopackApp.Build().Run();
            Application.ApplicationExit += Application_ApplicationExit;
            Process.GetCurrentProcess().Exited += Program_Exited;

            NotificationIcon.StartNotificationIcon();

            bool nodeExists = File.Exists("..\\node.exe");

            if (!nodeExists)
            {
                File.Copy(".\\redis\\node.exe", "..\\node.exe");
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
        
        private static async Task UpdateMyApp()
        {
            var mgr = new UpdateManager("E:\\Libraries\\Documents\\Projects\\MagpyServerWindows\\Releases");

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
                return; // no update available

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
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
