﻿using System;
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

        static async Task MainInner(string[] args)
        {
            LoggingManager.Init();
            VelopackApp.Build()
                .WithAfterInstallFastCallback((v) =>
                {
                    AutoStartupSetup.EnableAutoStartup();
                })
                .WithBeforeUninstallFastCallback((v) =>
                {
                    AutoStartupSetup.DisableAutoStartup();
                })
                .WithFirstRun((v) => {
                    ServerManager.OpenWebInterface();
                })
            .Run(LoggingManager.SerilogToMicrosoftLogger(LoggingManager.LoggerInstaller));

            await UpdateMyApp();

            bool nodefound = NodeExeManager.VerifyNodeExe();

            if (!nodefound)
            {
                throw new Exception("Node executable not found.");
            }

            child = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = NodeExeManager.NodePath,
                    Arguments = NodeExeManager.JsEntryFilePath,
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

            Application.ApplicationExit += Application_ApplicationExit;
            Process.GetCurrentProcess().Exited += Program_Exited;

            NotificationIcon.StartNotificationIcon();

            Application.Run();
        }

        static async Task Main(string[] args)
        {
            Mutex mutex = new Mutex(false, Constants.SINGLE_APP_INSTANCE_MUTEX_ID);
            try
            {
                if (mutex.WaitOne(0, false))
                {
                    await MainInner(args);
                }
                else
                {
                    ServerManager.OpenWebInterface();
                    Application.Exit();
                }
            }
            catch (Exception e)
            {
                LoggingManager.LoggerWinApp.Error(e, "Error launching app");
#if DEBUG
                Console.ReadKey();
#endif
            }
            finally
            {
                if (mutex != null)
                {
                    mutex.Close();
                    mutex = null;
                }
            }
        }
        
        private static async Task UpdateMyApp()
        {
            var mgr = new UpdateManager("E:\\Libraries\\Documents\\Projects\\MagpyServerWindows\\Releases");

            // check is app installed
            if (!mgr.IsInstalled)
            {
                return; // app not installed
            }

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                return; // no update available
            }

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
            LoggingManager.LoggerNode.Debug(e.Data);
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
